using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using CvManagementApp.Models;

namespace Views.Pages.Candidates
{
    public class DetailsModel : PageModel
    {
        private readonly CvManagementApp.Models.CvManagementDbContext _context;

        public DetailsModel(CvManagementApp.Models.CvManagementDbContext context)
        {
            _context = context;
        }

        public Candidate Candidate { get; set; } = default!;
        public IList<Degree> Degree { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var candidate = await _context.Candidates.FirstOrDefaultAsync(m => m.Id == id);
            Degree = await _context.Degrees.ToListAsync();

            if (candidate == null)
            {
                return NotFound();
            }
            else
            {
                Candidate = candidate;
            }
            return Page();
        }
    }
}
