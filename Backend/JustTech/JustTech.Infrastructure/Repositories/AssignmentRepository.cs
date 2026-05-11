using JustTech.Core.Entities;
using JustTech.Core.Interfaces;
using JustTech.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace JustTech.Infrastructure.Repositories
{
    public class AssignmentRepository : Repository<Assignment>, IAssignmentRepository
    {
        public AssignmentRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Assignment>> GetAssignmentsByRoundIdAsync(int roundId)
        {
            return await _context.Assignments
                .Where(a => a.RoundId == roundId && a.DeletedAt == null)
                .OrderBy(a => a.DueDate)
                .ToListAsync();
        }

        public async Task<Assignment?> GetAssignmentWithSubmissionsAsync(int assignmentId)
        {
            return await _context.Assignments
                .Include(a => a.Submissions)
                .FirstOrDefaultAsync(a => a.Id == assignmentId && a.DeletedAt == null);
        }
    }
}
