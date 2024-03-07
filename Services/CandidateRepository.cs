﻿using CvManagementApp.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;

namespace CvManagementApp.Services
{
    public class CandidateRepository : Repository<Candidate>, ICandidateRepository
    {
        private readonly CvManagementDbContext _context;

        public CandidateRepository(CvManagementDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Candidate>> GetAllAsyncWithDegrees()
        {
            return await _context.Candidates.Include(c => c.Degrees).ToListAsync();
        }

        public async Task UpdateCandidateAsync(Candidate candidate, IFormFile uploadedDocument)
        {
            var existingCandidate = await _context.Candidates.Include(c => c.Degrees).FirstOrDefaultAsync(c => c.Id == candidate.Id);
            if (existingCandidate == null)
            {
                throw new ArgumentException("Candidate not found.");
            }

            // Map properties excluding CV data
            existingCandidate.FirstName = candidate.FirstName;
            existingCandidate.LastName = candidate.LastName;
            // Continue mapping other properties...

            // Update CV only if a new valid file is uploaded
            if (uploadedDocument != null && uploadedDocument.Length > 0)
            {
                using var memoryStream = new MemoryStream();
                await uploadedDocument.CopyToAsync(memoryStream);
                existingCandidate.CV = memoryStream.ToArray();
                existingCandidate.CVFileName = uploadedDocument.FileName;
                existingCandidate.CVMimeType = uploadedDocument.ContentType;
            }

            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Degree>> GetAllUsedUniqueDegreesAsync()
        {
            var usedDegrees = await _context.Candidates
                                            .AsNoTracking()
                                            .Include(c => c.Degrees)
                                            .Where(c => c.Degrees != null)
                                            .SelectMany(c => c.Degrees!)
                                            .Distinct()
                                            .ToListAsync();
            return usedDegrees;
        }

        public async Task<Candidate?> GetByCandidateIdDegreeListAsync(int candidateId)
        {
            return await _context.Candidates
                                 .AsNoTracking()
                                 .Include(c => c.Degrees)
                                 .FirstOrDefaultAsync(c => c.Id == candidateId);
        }

        public async Task SetCandidateDegreesAsync(int candidateId, IEnumerable<int> degreeIds)
        {
            var candidate = await _context.Candidates
                                          .Include(c => c.Degrees)
                                          .FirstOrDefaultAsync(c => c.Id == candidateId);

            if (candidate == null)
            {
                throw new ArgumentException($"Candidate with ID {candidateId} not found.");
            }

            var degreesToSet = await _context.Degrees
                                             .Where(degree => degreeIds.Contains(degree.Id))
                                             .ToListAsync();

            if (candidate.Degrees != null)
            {
                candidate.Degrees.Clear();
            }
            else
            {
                candidate.Degrees = new List<Degree>();
            }

            foreach (var degree in degreesToSet)
            {
                candidate.Degrees.Add(degree);
            }
            await _context.SaveChangesAsync();
        }
    }
}
