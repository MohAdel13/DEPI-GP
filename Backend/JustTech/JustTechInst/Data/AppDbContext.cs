using Microsoft.EntityFrameworkCore;
using JustTech.Models;

namespace JustTech.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }
        public DbSet<AppUser> Users { get; set; }
        public DbSet<Course> Courses { get; set; }
        public DbSet<Lecture> Lectures { get; set; }

        public DbSet<Section> Sections { get; set; }
    }
}