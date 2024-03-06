using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using CvManagementApp.Models;

namespace Views.Pages.Degrees
{
    public class CreateModel : PageModel
    {
        private readonly CvManagementApp.Models.CvManagementDbContext _context;

        public CreateModel(CvManagementApp.Models.CvManagementDbContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        [BindProperty]
        public Degree Degree { get; set; } = default!;

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Degrees.Add(Degree);
            await _context.SaveChangesAsync();

            return RedirectToPage("../Index");
        }
    }
}
