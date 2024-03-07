using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using CvManagementApp.Models;
using CvManagementApp.Services;

namespace Views.Pages.Degrees
{
    public class DetailsModel : PageModel
    {
        private readonly IRepository<Degree> _degreeRepository;

        public DetailsModel(IRepository<Degree> degreeRepository)
        {
            _degreeRepository = degreeRepository;
        }

        public Degree Degree { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Degree = await _degreeRepository.GetByIdAsync(id.Value);

            if (Degree == null)
            {
                return NotFound();
            }
            return Page();
        }
    }
}
