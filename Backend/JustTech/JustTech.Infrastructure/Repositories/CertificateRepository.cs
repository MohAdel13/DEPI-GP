using JustTech.Core.Entities;
using JustTech.Core.Interfaces;
using JustTech.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace JustTech.Infrastructure.Repositories
{
    public class CertificateRepository : Repository<Certificate>, ICertificateRepository
    {
        public CertificateRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<Certificate?> GetCertificateByStudentAndRoundAsync(int studentId, int roundId)
        {
            return await _context.Certificates
                .FirstOrDefaultAsync(c => c.StudentId == studentId
                    && c.RoundId == roundId
                    && c.DeletedAt == null);
        }
        public async Task<IEnumerable<Certificate>> GetCertificatesByStudentIdAsync(int studentId)
        {
            return await _context.Certificates
                .Where(c => c.StudentId == studentId && c.DeletedAt == null)
                .Include(c => c.Round)
                    .ThenInclude(r => r.Course)
                .Include(c => c.Student)
                .ToListAsync();
        }

        public async Task<IEnumerable<Certificate>> GetCertificatesByRoundIdAsync(int roundId)
        {
            return await _context.Certificates
                .Where(c => c.RoundId == roundId && c.DeletedAt == null)
                .Include(c => c.Student)
                .Include(c => c.Round)
                    .ThenInclude(r => r.Course)
                .ToListAsync();
        }

        public async Task<bool> HasCertificateAsync(int studentId, int roundId)
        {
            return await _context.Certificates
                .AnyAsync(c => c.StudentId == studentId
                    && c.RoundId == roundId
                    && c.DeletedAt == null);
        }

    }
}
