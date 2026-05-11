using JustTech.Core.Entities;
using JustTech.Core.Interfaces;
using JustTech.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace JustTech.Infrastructure.Repositories
{
    public class MaterialRepository : Repository<Material>, IMaterialRepository
    {
        public MaterialRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Material>> GetMaterialsByLectureIdAsync(int lectureId)
        {
            return await _context.Materials
                .Where(m => m.LectureId == lectureId && m.DeletedAt == null)
                .ToListAsync();
        }
    }
}
