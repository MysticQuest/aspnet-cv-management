using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore;

namespace CvManagementApp.Models
{
    // Design-time DB factory for Razor HTML scaffolding
    public class CvManagementDbContextFactory : IDesignTimeDbContextFactory<CvManagementDbContext>
    {
        public CvManagementDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<CvManagementDbContext>();
            optionsBuilder.UseInMemoryDatabase("CvManagementDb");

            return new CvManagementDbContext(optionsBuilder.Options);
        }
    }
}
