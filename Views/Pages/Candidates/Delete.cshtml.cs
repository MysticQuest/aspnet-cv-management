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
    public class DeleteModel : PageModel
    {
        private readonly ICandidateRepository candidateRepository;

        public DeleteModel(ICandidateRepository candidateRepository)
        {
            this.candidateRepository = candidateRepository;
        }

        [BindProperty]
        public Candidate Candidate { get; set; } = default!;
        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Candidate = await candidateRepository.GetByCandidateIdDegreeListAsync(id.Value);

            if (Candidate == null)
            {
                return NotFound();
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null || Candidate == null)
            {
                return NotFound();
            }
            await candidateRepository.DeleteAsync(Candidate.Id);
            return RedirectToPage("../Index");
        }
    }
}
