﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CvManagementApp.Models;

namespace Views.Pages.Candidates
{
    public class EditModel : PageModel
    {
        private readonly CvManagementApp.Models.CvManagementDbContext _context;

        public EditModel(CvManagementApp.Models.CvManagementDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Candidate Candidate { get; set; } = default!;
        [BindProperty]
        public IFormFile UploadedDocument { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var candidate =  await _context.Candidates.FirstOrDefaultAsync(m => m.Id == id);
            if (candidate == null)
            {
                return NotFound();
            }
            Candidate = candidate;
            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (UploadedDocument != null && UploadedDocument.Length > 0)
            {
                var allowedContentTypes = new List<string>
                {
                    "application/pdf",
                    "application/msword",
                    "application/vnd.openxmlformats-officedocument.wordprocessingml.document"
                };

                if (!allowedContentTypes.Contains(UploadedDocument.ContentType))
                {
                    ModelState.AddModelError("UploadedDocument", "Only PDF and Word documents are allowed.");
                }
                else
                {
                    using var memoryStream = new MemoryStream();
                    await UploadedDocument.CopyToAsync(memoryStream);
                    Candidate.CV = memoryStream.ToArray();
                    Candidate.CVFileName = UploadedDocument.FileName;
                    Candidate.CVMimeType = UploadedDocument.ContentType;
                }
            }

            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Attach(Candidate).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CandidateExists(Candidate.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("../Index");
        }

        private bool CandidateExists(int id)
        {
            return _context.Candidates.Any(e => e.Id == id);
        }
    }
}
