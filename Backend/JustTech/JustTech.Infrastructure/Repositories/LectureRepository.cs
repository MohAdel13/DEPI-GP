using JustTech.Core.Entities;
using JustTech.Core.Interfaces;
using JustTech.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;


namespace JustTech.Infrastructure.Repositories
{
    public class LectureRepository : Repository<Lecture>, ILectureRepository
    {
        public LectureRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Lecture>> GetLecturesByRoundIdAsync(int roundId)
        {
            return await _context.Lectures
                .Where(l => l.RoundId == roundId && l.DeletedAt == null)
                .OrderBy(l => l.CreatedAt)
                .ToListAsync();
        }

        public async Task<Lecture?> GetLectureWithMaterialsAsync(int lectureId)
        {
            return await _context.Lectures
                .Include(l => l.Materials)
                .FirstOrDefaultAsync(l => l.Id == lectureId && l.DeletedAt == null);
        }
    }
}
