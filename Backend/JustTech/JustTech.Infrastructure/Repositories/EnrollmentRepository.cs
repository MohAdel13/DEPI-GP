using JustTech.Core.Entities;
using JustTech.Core.Interfaces;
using JustTech.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace JustTech.Infrastructure.Repositories
{
    public class EnrollmentRepository : Repository<Enrollment>, IEnrollmentRepository
    {
        public EnrollmentRepository(AppDbContext context) : base(context)
        {
        }
        public async Task<IEnumerable<Enrollment>> GetEnrollmentsByStudentIdAsync(int studentId)
        {
            return await _context.Enrollments
                 .Where(e => e.StudentId == studentId && e.DeletedAt == null)
                 .Include(e => e.Round)
                 .ThenInclude(r => r.Course)
                 .ToListAsync();
        }

        public async Task<IEnumerable<Enrollment>> GetEnrollmentsByRoundIdAsync(int roundId)
        {
            return await _context.Enrollments
                 .Where(e => e.RoundId == roundId && e.DeletedAt == null)
                 .Include(e => e.Student)
                 .ToListAsync();
        }

        public async Task<Enrollment?> GetEnrollmentByStudentAndRoundAsync(int studentId, int roundId)
        {
            return await _context.Enrollments
                .FirstOrDefaultAsync(e => e.StudentId == studentId
                && e.RoundId == roundId
                && e.DeletedAt == null);
        }

        

       

        public async Task<bool> IsStudentEnrolledAsync(int studentId, int roundId)
        {
            return await _context.Enrollments
                .AnyAsync(e => e.StudentId == studentId
                && e.RoundId == roundId
                && e.DeletedAt == null);
        }
    }
}
