using CvManagementApp.Models;
using CvManagementApp.Services;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CvManagementApp.Services
{
    public interface ICandidateRepository : IRepository<Candidate>
    {
        Task<IEnumerable<Candidate>> GetAllAsyncWithDegrees();
        Task UpdateCandidateAsync(Candidate candidate, IFormFile uploadedDocument);
        Task<Candidate?> GetByCandidateIdDegreeListAsync(int candidateId);
        Task<IEnumerable<Degree>> GetAllUsedUniqueDegreesAsync();
        Task SetCandidateDegreesAsync(int candidateId, IEnumerable<int> degreeIds);
    }
}
