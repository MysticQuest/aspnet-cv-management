﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using CvManagementApp.Models;

namespace Views.Pages.Candidates
{
    public class CreateModel : PageModel
    {
        private readonly CvManagementApp.Models.CvManagementDbContext _context;

        public CreateModel(CvManagementApp.Models.CvManagementDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Candidate Candidate { get; set; } = default!;

        [BindProperty]
        public IFormFile UploadedDocument { get; set; }

        public IActionResult OnGet()
        {
            return Page();
        }


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

            _context.Candidates.Add(Candidate);
            await _context.SaveChangesAsync();

            return RedirectToPage("../Index");
        }
    }
}
