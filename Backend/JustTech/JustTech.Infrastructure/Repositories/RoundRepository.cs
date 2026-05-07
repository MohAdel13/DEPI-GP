using JustTech.Core.Entities;
using JustTech.Core.Interfaces;
using JustTech.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace JustTech.Infrastructure.Repositories
{
    public class RoundRepository : Repository<Round>, IRoundRepository
    {
        public RoundRepository(AppDbContext context) : base(context)
        { }

        public async Task<IEnumerable<Round>> GetRoundsByCourseIdAsync(int courseId)
        {
            return await _context.Rounds
                 .Where(r => r.CourseId == courseId && r.DeletedAt == null)
                 .Include(r => r.Course)
                 .Include(r => r.Instructor)
                 .ToListAsync();
        }

        public async Task<Round?> GetRoundWithEnrollmentsAsync(int roundId)
        {
            return await _context.Rounds
                .Include(r => r.Enrollments)
                .ThenInclude(e => e.Student)
                .FirstOrDefaultAsync(r => r.Id == roundId && r.DeletedAt == null);
        }

        public async Task<IEnumerable<Round>> GetActiveRoundsAsync()
        {
            return await _context.Rounds
                .Where(r => r.Status == "in progress" && r.DeletedAt == null)
                .Include(r => r.Course)
                .ToListAsync();
        }        
    }
}
