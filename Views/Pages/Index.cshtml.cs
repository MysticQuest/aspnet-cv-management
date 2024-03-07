using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using CvManagementApp.Models;
using CvManagementApp.Services;
using Microsoft.EntityFrameworkCore;

namespace Views.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ICandidateRepository _candidateRepository;
        private readonly IRepository<Degree> _degreeRepository;

        public IndexModel(ICandidateRepository candidateRepository, IRepository<Degree> degreeRepository)
        {
            _candidateRepository = candidateRepository;
            _degreeRepository = degreeRepository;

            Candidate = new List<Candidate>();
            Degree = new List<Degree>();
        }

        public IList<Candidate> Candidate { get; private set; }
        public IList<Degree> Degree { get; private set; }

        public async Task OnGetAsync()
        {
            Candidate = (await _candidateRepository.GetAllAsyncWithDegrees()).ToList();
            Degree = (await _degreeRepository.GetAllAsync()).ToList();
        }

        public async Task<IActionResult> OnGetDownloadCVAsync(int id)
        {
            var candidate = await _candidateRepository.GetByIdAsync(id);

            if (candidate == null || candidate.CV == null || candidate.CV.Length == 0)
            {
                return NotFound();
            }
            string mimeType = candidate.CVMimeType ?? "application/octet-stream";
            return File(candidate.CV, mimeType, candidate.CVFileName);
        }

        public async Task<IActionResult> OnGetDeleteUnassociatedDegreesAsync()
        {
            var usedDegreeIds = new HashSet<int>((await _candidateRepository.GetAllUsedUniqueDegreesAsync()).Select(d => d.Id));

            foreach (var degree in (await _degreeRepository.GetAllAsync()).Where(d => !usedDegreeIds.Contains(d.Id)))
            {
                await _degreeRepository.DeleteAsync(degree.Id);
            }

            return RedirectToPage();
        }
    }
}
