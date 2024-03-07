﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CvManagementApp.Models;
using CvManagementApp.Services;

namespace Views.Pages.Candidates
{
    public class EditModel : PageModel
    {
        private readonly ICandidateRepository _candidateRepository;
        private readonly IRepository<Degree> _degreeRepository;

        public EditModel(ICandidateRepository candidateRepository, IRepository<Degree> degreeRepository)
        {
            _candidateRepository = candidateRepository;
            _degreeRepository = degreeRepository;
        }

        [BindProperty]
        public Candidate Candidate { get; set; } = default!;
        [BindProperty]
        public IFormFile UploadedDocument { get; set; }
        [BindProperty]
        public int[] SelectedDegreeIds { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Candidate = await _candidateRepository.GetByCandidateIdDegreeListAsync(id.Value);

            if (Candidate == null)
            {
                return NotFound();
            }

            await LoadFormDataAsync(id);
            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (UploadedDocument != null && UploadedDocument.Length > 0 && !IsValidFileType(UploadedDocument.ContentType))
            {
                ModelState.AddModelError("UploadedDocument", "Only PDF and Word documents are allowed.");
                await LoadFormDataAsync(Candidate.Id);
                return Page();
            }

            await _candidateRepository.UpdateCandidateAsync(Candidate, UploadedDocument);
            await _candidateRepository.SetCandidateDegreesAsync(Candidate.Id, SelectedDegreeIds);

            if (!ModelState.IsValid)
            {
                await LoadFormDataAsync();
                return Page();
            }

            return RedirectToPage("../Index");
        }

        private async Task LoadFormDataAsync(int? candidateId = null)
        {
            if (candidateId.HasValue)
            {
                Candidate = await _candidateRepository.GetByCandidateIdDegreeListAsync(candidateId.Value);
            }

            var allDegrees = await _degreeRepository.GetAllAsync();
            var selectedIds = Candidate?.Degrees.Select(d => d.Id).ToList() ?? new List<int>();

            ViewData["AllDegrees"] = allDegrees.Select(d => new SelectListItem
            {
                Value = d.Id.ToString(),
                Text = d.Name,
                Selected = selectedIds.Contains(d.Id)
            }).ToList();
        }

        private bool IsValidFileType(string contentType)
        {
            var allowedContentTypes = new List<string>
            {
                "application/pdf",
                "application/msword",
                "application/vnd.openxmlformats-officedocument.wordprocessingml.document"
            };
            return allowedContentTypes.Contains(contentType);
        }
    }
}
