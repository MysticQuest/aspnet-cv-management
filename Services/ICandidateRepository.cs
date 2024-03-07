using CvManagementApp.Models;
using CvManagementApp.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CvManagementApp.Services
{
    internal interface ICandidateRepository : IRepository<Candidate>
    {
        Task<Candidate?> GetByCandidateIdDegreeListAsync(int candidateId);
        Task<IEnumerable<Degree>> GetAllUsedUniqueDegreesAsync();
    }
}
