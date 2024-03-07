using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using CvManagementApp.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using CvManagementApp.Services;

namespace Views.Pages.Candidates
{
    public class CreateModel : PageModel
    {
        private readonly ICandidateRepository _candidateRepository;
        private readonly IRepository<Degree> _degreeRepository;

        public CreateModel(ICandidateRepository candidateRepository, IRepository<Degree> degreeRepository)
        {
            _candidateRepository = candidateRepository;
            _degreeRepository = degreeRepository;
        }

        [BindProperty]
        public Candidate Candidate { get; set; } = default!;

        [BindProperty]
        public int[] SelectedDegreeIds { get; set; }

        [BindProperty]
        public IFormFile UploadedDocument { get; set; }

        public async Task<IActionResult> OnGet()
        {
            ViewData["DegreeId"] = new SelectList(await _degreeRepository.GetAllAsync(), "Id", "Name");
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (UploadedDocument != null && UploadedDocument.Length > 0)
            {
                if (!IsValidFileType(UploadedDocument.ContentType))
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
                await LoadFormDataAsync();
                return Page();
            }

            await _candidateRepository.AddAsync(Candidate);
            if (SelectedDegreeIds != null && SelectedDegreeIds.Any())
            {
                await _candidateRepository.SetCandidateDegreesAsync(Candidate.Id, SelectedDegreeIds);
            }

            return RedirectToPage("../Index");
        }


        private async Task LoadFormDataAsync()
        {
            var degrees = await _degreeRepository.GetAllAsync();
            ViewData["DegreeId"] = new SelectList(degrees, "Id", "Name");
            ViewData["SelectedDegreeIds"] = SelectedDegreeIds;
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
