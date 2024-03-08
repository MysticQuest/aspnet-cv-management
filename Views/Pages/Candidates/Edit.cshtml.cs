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
        public IFormFile? UploadedDocument { get; set; } = default!;
        [BindProperty]
        public int[] SelectedDegreeIds { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Candidate = await _candidateRepository.GetByCandidateIdDegreeListAsync(id.Value)
                ?? new Candidate();

            if (Candidate == null)
            {
                return NotFound();
            }

            await LoadFormDataAsync(id);
            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (UploadedDocument != null && UploadedDocument.Length > 0 && !IsValidFileType(UploadedDocument.ContentType))
            {
                ModelState.AddModelError("UploadedDocument", "Only PDF and Word documents are allowed.");
                await LoadFormDataAsync(Candidate.Id);
                return Page();
            }

            await _candidateRepository.UpdateCandidateAsync(Candidate, UploadedDocument);
            await _candidateRepository.SetCandidateDegreesAsync(Candidate.Id, SelectedDegreeIds);

            if (!ModelState.IsValid)
            {
                await LoadFormDataAsync(Candidate.Id);
                return Page();
            }

            return RedirectToPage("../Index");
        }

        private async Task LoadFormDataAsync(int? candidateId = null)
        {
            if (candidateId.HasValue)
            {
                Candidate = await _candidateRepository.GetByCandidateIdDegreeListAsync(candidateId.Value) ?? new Candidate();
            }

            var allDegrees = await _degreeRepository.GetAllAsync();
            ViewData["AllDegrees"] = new SelectList(allDegrees, "Id", "Name");

            // Convert SelectedDegreeIds to List<int> for consistent handling.
            var selectedDegreeIdsList = SelectedDegreeIds?.ToList() ?? new List<int>();

            // Determine the IDs to select based on whether it's a form resubmission.
            var currentlySelectedIds = HttpContext.Request.Method.Equals("POST", StringComparison.OrdinalIgnoreCase)
                ? selectedDegreeIdsList
                : (candidateId.HasValue
                    ? Candidate.Degrees?.Select(d => d.Id).ToList() ?? new List<int>()
                    : new List<int>());

            ViewData["SelectedDegreeIds"] = currentlySelectedIds;
        }

        private bool IsValidFileType(string contentType)
        {
            var allowedContentTypes = new List<string>
            {
                "application/pdf",
                "application/msword",
                "application/vnd.openxmlformats-officedocument.wordprocessingml.document"
            };
            return allowedContentTypes.Contains(contentType);
        }
    }
}
