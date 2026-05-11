using JustTech.Core.Entities;
using JustTech.Core.Interfaces;
using JustTech.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace JustTech.Infrastructure.Repositories
{
    public class InstructorRepository : Repository<Instructor>, IInstructorRepository
    {
        public InstructorRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<Instructor?> GetByEmailAsync(string email)
        {
            return await _context.Instructors
        .FirstOrDefaultAsync(i => i.Email == email && i.DeletedAt == null);
        }

        public async Task<IEnumerable<Instructor>> GetInstructorsByCityAsync(string city)
        {
            return await _context.Instructors
        .Where(i => i.City == city && i.DeletedAt == null)
        .ToListAsync();

        }

        public async Task<IEnumerable<Instructor>> GetInstructorsByProfessionAsync(string profession)
        {
            return await _context.Instructors
      .Where(i => i.Profession == profession && i.DeletedAt == null)
      .ToListAsync();

        }
    }
}
