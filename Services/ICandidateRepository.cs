using CvManagementApp.Models;
using CvManagementApp.Services;
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
        Task<Candidate?> GetByCandidateIdDegreeListAsync(int candidateId);
        Task<IEnumerable<Degree>> GetAllUsedUniqueDegreesAsync();
        Task SetCandidateDegreesAsync(int candidateId, IEnumerable<int> degreeIds);
    }
}
