using CvManagementApp.Models;
using CvManagementApp.Services;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

            var degreesToSet = await _context.Degrees
                                             .Where(degree => degreeIds.Contains(degree.Id))
                                             .ToListAsync();

            candidate.Degrees.Clear();
            foreach (var degree in degreesToSet)
            {
                candidate.Degrees.Add(degree);
            }
            await _context.SaveChangesAsync();
        }
    }
}
