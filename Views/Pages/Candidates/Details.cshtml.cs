using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using CvManagementApp.Models;
using CvManagementApp.Services;

namespace Views.Pages.Candidates
{
    public class DetailsModel : PageModel
    {
        private readonly ICandidateRepository _candidateRepository;

        public DetailsModel(ICandidateRepository candidateRepository)
        {
            _candidateRepository = candidateRepository;
        }

        public Candidate Candidate { get; set; } = default!;

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

            return Page();
        }
    }
}
