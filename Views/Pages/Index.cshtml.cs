using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using CvManagementApp.Models;
using CvManagementApp.Services;

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

            return File(candidate.CV, candidate.CVMimeType, candidate.CVFileName);
        }

        public async Task<IActionResult> OnGetDeleteUnassociatedDegreesAsync()
        {
            var allDegrees = await _degreeRepository.GetAllAsync();
            var usedDegrees = await _candidateRepository.GetAllUsedUniqueDegreesAsync();

            var unassociatedDegrees = allDegrees.Except(usedDegrees).ToList();
            foreach (var degree in unassociatedDegrees)
            {
                await _degreeRepository.DeleteAsync(degree.Id);
            }

            return RedirectToPage();
        }
    }
}
