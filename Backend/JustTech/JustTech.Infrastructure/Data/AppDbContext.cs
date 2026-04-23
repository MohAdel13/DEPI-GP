using Microsoft.EntityFrameworkCore;
using JustTech.Core.Entities;
namespace JustTech.Infrastructure.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Student> Students { get; set; }
        public DbSet<Instructor> Instructors { get; set; }
        public DbSet<Course> Courses { get; set; }
        public DbSet<Round> Rounds { get; set; }
        public DbSet<Enrollment> Enrollments { get; set; }
        public DbSet<Lecture> Lectures { get; set; }
        public DbSet<Material> Materials { get; set; }
        public DbSet<Progress> Progresses { get; set; }
        public DbSet<Assignment> Assignments { get; set; }
        public DbSet<Submission> Submissions { get; set; }
        public DbSet<Certificate> Certificates { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}
