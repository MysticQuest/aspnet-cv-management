using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Reflection.Emit;

namespace CvManagementApp.Models
{
    public class CvManagementDbContext : DbContext
    {
        public CvManagementDbContext(DbContextOptions<CvManagementDbContext> options)
            : base(options)
        {
        }
    }
}