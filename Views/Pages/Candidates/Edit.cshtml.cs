using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CvManagementApp.Models;
using CvManagementApp.Services;

namespace Views.Pages.Candidates
{
    public class EditModel : PageModel
    {
        private readonly ICandidateRepository _candidateRepository;
        private readonly IRepository<Degree> _degreeRepository;

        public EditModel(ICandidateRepository candidateRepository, IRepository<Degree> degreeRepository)
        {
            _candidateRepository = candidateRepository;
            _degreeRepository = degreeRepository;
        }

        [BindProperty]
        public Candidate Candidate { get; set; } = default!;
        [BindProperty]
        public IFormFile UploadedDocument { get; set; }
        [BindProperty]
        public int[] SelectedDegreeIds { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Candidate = await _candidateRepository.GetByCandidateIdDegreeListAsync(id.Value);

            if (Candidate == null)
            {
                return NotFound();
            }

            var allDegrees = await _degreeRepository.GetAllAsync();
            ViewData["AllDegrees"] = allDegrees.Select(d => new SelectListItem
            {
                Value = d.Id.ToString(),
                Text = d.Name,
                Selected = Candidate.Degrees.Any(cd => cd.Id == d.Id)
            }).ToList();

            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
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
                        Candidate.CV = memoryStream.ToArray();
                        Candidate.CVFileName = UploadedDocument.FileName;
                        Candidate.CVMimeType = UploadedDocument.ContentType;
                    }
                }
            }

            if (!ModelState.IsValid)
            {
                return Page();
            }

            await _candidateRepository.UpdateAsync(Candidate);
            await _candidateRepository.SetCandidateDegreesAsync(Candidate.Id, SelectedDegreeIds);

            return RedirectToPage("../Index");
        }
    }
}
