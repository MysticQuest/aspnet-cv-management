﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CvManagementApp.Models;
using CvManagementApp.Services;

namespace Views.Pages.Degrees
{
    public class EditModel : PageModel
    {
        private readonly IRepository<Degree> _degreeRepository;

        public EditModel(IRepository<Degree> degreeRepository)
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

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var degrees = await _degreeRepository.GetAllAsync();
            var existingDegree = degrees.FirstOrDefault(d => d.Name.Equals(Degree.Name, StringComparison.OrdinalIgnoreCase) && d.Id != Degree.Id);

            if (existingDegree != null)
            {
                ModelState.AddModelError("Degree.Name", "Degree name must be unique.");
                return Page();
            }

            if (Degree == null)
            {
                return NotFound();
            }

            await _degreeRepository.UpdateAsync(Degree);

            return RedirectToPage("../Index");
        }


        private async Task<bool> DegreeExists(int id)
        {
            var degree = await _degreeRepository.GetByIdAsync(id);
            return degree != null;
        }
    }
}
