using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CvManagementApp.Models;

namespace Views.Pages.Candidates
{
    public class EditModel : PageModel
    {
        private readonly CvManagementApp.Models.CvManagementDbContext _context;

        public EditModel(CvManagementApp.Models.CvManagementDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Candidate Candidate { get; set; } = default!;
        [BindProperty]
        public IFormFile UploadedDocument { get; set; }
        public IList<Degree> Degree { get; set; }
        [BindProperty]
        public int[] SelectedDegreeIds { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Candidate = await _context.Candidates
                                      .Include(c => c.Degrees)
                                      .FirstOrDefaultAsync(m => m.Id == id.Value);

            if (Candidate == null)
            {
                return NotFound();
            }

            Degree = await _context.Degrees.ToListAsync();

            var candidateDegreeIds = Candidate.Degrees.Select(d => d.Id).ToList();
            ViewData["AllDegrees"] = Degree.Select(d => new SelectListItem
            {
                Value = d.Id.ToString(),
                Text = d.Name,
                Selected = candidateDegreeIds.Contains(d.Id)
            }).ToList();

            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            var existingCandidate = await _context.Candidates
                .Include(c => c.Degrees)
                .FirstOrDefaultAsync(c => c.Id == Candidate.Id);

            if (existingCandidate == null)
            {
                return NotFound("Candidate not found.");
            }

            _context.Entry(existingCandidate).CurrentValues.SetValues(Candidate);

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
                    using (var memoryStream = new MemoryStream())
                    {
                        await UploadedDocument.CopyToAsync(memoryStream);
                        existingCandidate.CV = memoryStream.ToArray();
                        existingCandidate.CVFileName = UploadedDocument.FileName;
                        existingCandidate.CVMimeType = UploadedDocument.ContentType;
                    }
                }
            }

            existingCandidate.Degrees.Clear();

            if (SelectedDegreeIds != null && SelectedDegreeIds.Any())
            {
                var selectedDegrees = await _context.Degrees
                    .Where(d => SelectedDegreeIds.Contains(d.Id))
                    .ToListAsync();

                foreach (var degree in selectedDegrees)
                {
                    existingCandidate.Degrees.Add(degree);
                }
            }

            if (!ModelState.IsValid)
            {
                return Page();
            }

            await _context.SaveChangesAsync();

            return RedirectToPage("../Index");
        }


        private bool CandidateExists(int id)
        {
            return _context.Candidates.Any(e => e.Id == id);
        }
    }
}
