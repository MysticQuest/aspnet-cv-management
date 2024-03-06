using Microsoft.EntityFrameworkCore;

namespace CvManagementApp.Models
{
    public class CvManagementDbContext : DbContext
    {
        public CvManagementDbContext(DbContextOptions<CvManagementDbContext> options)
            : base(options)
        {
        }

        public DbSet<Candidate> Candidates { get; set; }
        public DbSet<Degree> Degrees { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Candidate>()
                .HasMany(c => c.Degrees)
                .WithMany()
                .UsingEntity(j => j.ToTable("CandidateDegrees"));
        }
    }
}