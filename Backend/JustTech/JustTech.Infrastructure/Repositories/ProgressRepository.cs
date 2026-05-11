using JustTech.Core.Entities;
using JustTech.Core.Interfaces;
using JustTech.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;


namespace JustTech.Infrastructure.Repositories
{
    public class ProgressRepository : Repository<Progress>, IProgressRepository
    {
        public ProgressRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<Progress?> GetProgressByStudentAndLectureAsync(int studentId, int lectureId)
        {
            return await _context.Progresses
                .FirstOrDefaultAsync(p => p.StudentId == studentId
                    && p.LectureId == lectureId
                    && p.DeletedAt == null);
        }

        public async Task<IEnumerable<Progress>> GetProgressByStudentIdAsync(int studentId)
        {
            return await _context.Progresses
                .Where(p => p.StudentId == studentId && p.DeletedAt == null)
                .Include(p => p.Lecture)
                .ThenInclude(l => l.Round)
                .ToListAsync();
        }

        public async Task<IEnumerable<Progress>> GetProgressByLectureIdAsync(int lectureId)
        {
            return await _context.Progresses
                .Where(p => p.LectureId == lectureId && p.DeletedAt == null)
                .Include(p => p.Student)
                .ToListAsync();
        }

        public async Task<int> GetCompletedLecturesCountByStudentIdAsync(int studentId)
        {
            return await _context.Progresses
                .Where(p => p.StudentId == studentId
                    && p.IsCompleted == true
                    && p.DeletedAt == null)
                .CountAsync();
        }

        public async Task<double> GetStudentProgressPercentageAsync(int studentId, int roundId)
        {
            var totalLectures = await _context.Lectures
                .Where(l => l.RoundId == roundId && l.DeletedAt == null)
                .CountAsync();

            if (totalLectures == 0) return 0;

            var completedLectures = await _context.Progresses
                .Include(p => p.Lecture)
                .Where(p => p.StudentId == studentId
                    && p.Lecture.RoundId == roundId
                    && p.IsCompleted == true
                    && p.DeletedAt == null)
                .CountAsync();

            return (double)completedLectures / totalLectures * 100;
        }
    }
}
