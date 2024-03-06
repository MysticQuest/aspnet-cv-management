using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using CvManagementApp.Models;

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
        public IFormFile UploadedCV { get; set; }

        public IActionResult OnGet()
        {
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var allowedContentTypes = new List<string>
            {
                "application/pdf",
                "application/msword",
                "application/vnd.openxmlformats-officedocument.wordprocessingml.document"
            };

            if (!allowedContentTypes.Contains(UploadedCV.ContentType))
            {
                ModelState.AddModelError("UploadedCV", "Only PDF and Word documents are allowed.");
            }
            else
            {
                // If the file is valid, read it into a byte array.
                using (var memoryStream = new MemoryStream())
                {
                    await UploadedCV.CopyToAsync(memoryStream);
                    Candidate.CV = memoryStream.ToArray();
                }
            }
            
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Candidates.Add(Candidate);
            await _context.SaveChangesAsync();

            return RedirectToPage("../Index");
        }
    }
}
