using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using CvManagementApp.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace Views.Pages.Candidates
{
    public class CreateModel : PageModel
    {
        private readonly CvManagementApp.Models.CvManagementDbContext _context;

        public CreateModel(CvManagementApp.Models.CvManagementDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Candidate Candidate { get; set; } = default!;

        [BindProperty]
        public int[] SelectedDegreeIds { get; set; }

        [BindProperty]
        public IFormFile UploadedDocument { get; set; }

        public IActionResult OnGet()
        {
            ViewData["DegreeId"] = new SelectList(_context.Degrees, "Id", "Name");
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var degreesToAdd = new List<Degree>();

            if (SelectedDegreeIds != null && SelectedDegreeIds.Length > 0)
            {
                degreesToAdd = await _context.Degrees
                    .Where(d => SelectedDegreeIds.Contains(d.Id))
                    .ToListAsync();
            }

            if (UploadedDocument != null && UploadedDocument.Length > 0)
            {
                var allowedContentTypes = new List<string>
                {
                    "application/pdf",
                    "application/msword",
                    "application/vnd.openxmlformats-officedocument.wordprocessingml.document"
                };

                if (!allowedContentTypes.Contains(UploadedDocument.ContentType))
                {
                    ModelState.AddModelError("UploadedDocument", "Only PDF and Word documents are allowed.");
                }
                else
                {
                    using var memoryStream = new MemoryStream();
                    await UploadedDocument.CopyToAsync(memoryStream);
                    Candidate.CV = memoryStream.ToArray();
                    Candidate.CVFileName = UploadedDocument.FileName; 
                    Candidate.CVMimeType = UploadedDocument.ContentType; 
                }
            }

            if (!ModelState.IsValid)
            {
                ViewData["DegreeId"] = new SelectList(_context.Degrees, "Id", "Name");
                return Page();
            }

            _context.Candidates.Add(Candidate);

            foreach (var degree in degreesToAdd)
            {
                Candidate.Degrees.Add(degree);
            }

            await _context.SaveChangesAsync();

            return RedirectToPage("../Index");
        }
    }
}
