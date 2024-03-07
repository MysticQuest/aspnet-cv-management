using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using CvManagementApp.Models;
using CvManagementApp.Services;

namespace Views.Pages.Degrees
{
    public class DeleteModel : PageModel
    {
        private readonly IRepository<Degree> _degreeRepository;

        public DeleteModel(IRepository<Degree> degreeRepository)
        {
            _degreeRepository = degreeRepository;
        }

        [BindProperty]
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

        public async Task<IActionResult> OnPostAsync()
        {
            if (Degree?.Id == null)
            {
                return NotFound();
            }

            await _degreeRepository.DeleteAsync(Degree.Id);

            return RedirectToPage("../Index");
        }
    }
}
