using JustTech.Core.DTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace JustTech.Business.Services
{
    public interface ICertificateService
    {
        Task<IEnumerable<CertificateDto>> GetAllAsync();
        Task<CertificateDto?> GetByIdAsync(int id);
        Task<IEnumerable<CertificateDto>> GetCertificatesByStudentIdAsync(int studentId);
        Task<IEnumerable<CertificateDto>> GetCertificatesByRoundIdAsync(int roundId);
        Task<CertificateDto?> GetCertificateByStudentAndRoundAsync(int studentId, int roundId);
        Task<CertificateDto?> GenerateCertificateAsync(GenerateCertificateDto generateDto);
        Task<CertificateDto?> UpdateCertificateAsync(int id, UpdateCertificateDto updateDto);
        Task<bool> DeleteAsync(int id);
        Task<bool> HasCertificateAsync(int studentId, int roundId);
    }
}
