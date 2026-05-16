using JustTech.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace JustTech.Core.Interfaces
{
    public interface ICertificateRepository : IRepository<Certificate>
    {
        Task<Certificate?> GetByIdAsync(int id);
        Task<Certificate?> GetCertificateByStudentAndRoundAsync(int studentId, int roundId);
        Task<IEnumerable<Certificate>> GetCertificatesByStudentIdAsync(int studentId);
        Task<IEnumerable<Certificate>> GetCertificatesByRoundIdAsync(int roundId);
        Task<bool> HasCertificateAsync(int studentId, int roundId);
    }
}
