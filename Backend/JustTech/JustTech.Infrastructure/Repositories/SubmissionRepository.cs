using JustTech.Core.Entities;
using JustTech.Core.Interfaces;
using JustTech.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace JustTech.Infrastructure.Repositories
{
    public class SubmissionRepository : Repository<Submission>, ISubmissionRepository
    {
        public SubmissionRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Submission>> GetSubmissionsByAssignmentIdAsync(int assignmentId)
        {
            return await _context.Submissions
                .Where(s => s.AssignmentId == assignmentId && s.DeletedAt == null)
                .Include(s => s.Student)
                .ToListAsync();
        }

        public async Task<IEnumerable<Submission>> GetSubmissionsByStudentIdAsync(int studentId)
        {
            return await _context.Submissions
                .Where(s => s.StudentId == studentId && s.DeletedAt == null)
                .Include(s => s.Assignment)
                .ToListAsync();
        }

        public async Task<Submission?> GetSubmissionByAssignmentAndStudentAsync(int assignmentId, int studentId)
        {
            return await _context.Submissions
                .FirstOrDefaultAsync(s => s.AssignmentId == assignmentId
                    && s.StudentId == studentId
                    && s.DeletedAt == null);
        }

        public async Task<IEnumerable<Submission>> GetSubmissionsByStatusAsync(string status)
        {
            return await _context.Submissions
                .Where(s => s.Status == status && s.DeletedAt == null)
                .Include(s => s.Assignment)
                .Include(s => s.Student)
                .ToListAsync();
        }

        public Task<IEnumerable<Assignment>> GetAssignmentsByRoundIdAsync(int roundId)
        {
            throw new NotImplementedException();
        }

        public Task<Assignment?> GetAssignmentWithSubmissionsAsync(int assignmentId)
        {
            throw new NotImplementedException();
        }
    }
}
