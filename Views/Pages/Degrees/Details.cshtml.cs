using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using CvManagementApp.Models;

namespace Views.Pages.Degrees
{
    public class DetailsModel : PageModel
    {
        private readonly CvManagementApp.Models.CvManagementDbContext _context;

        public DetailsModel(CvManagementApp.Models.CvManagementDbContext context)
        {
            _context = context;
        }

        public Degree Degree { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var degree = await _context.Degrees.FirstOrDefaultAsync(m => m.Id == id);
            if (degree == null)
            {
                return NotFound();
            }
            else
            {
                Degree = degree;
            }
            return Page();
        }
    }
}
