using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using CvManagementApp.Models;
using CvManagementApp.Services;

namespace Views.Pages.Degrees
{
    public class CreateModel : PageModel
    {
        private readonly IRepository<Degree> _degreeRepository;

        public CreateModel(IRepository<Degree> degreeRepository)
        {
            _degreeRepository = degreeRepository;
        }

        [BindProperty]
        public Degree Degree { get; set; } = default!;

        public IActionResult OnGet()
        {
            return Page();
        }

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var existingDegrees = await _degreeRepository.GetAllAsync();
            if (existingDegrees.Any(d => d.Name == Degree.Name))
            {
                ModelState.AddModelError("Degree.Name", "Degree name must be unique.");
                return Page();
            }

            await _degreeRepository.AddAsync(Degree);

            return RedirectToPage("../Index");
        }
    }
}
