using JustTech.Core.DTOs;

namespace JustTech.Business.Services
{
    public interface IEnrollmentService
    {
        Task<IEnumerable<EnrollmentDto>> GetAllAsync();
        Task<EnrollmentDto> GetByIdAsync(int id);
        Task<EnrollmentDto> EnrollStudentAsync(CreateEnrollmentDto createDto);
        Task<EnrollmentDto> UpdateEnrollmentStatusAsync(int id, UpdateEnrollmentStatusDto updateDto);
        Task DeleteAsync(int id);
        Task<IEnumerable<EnrollmentDto>> GetEnrollmentsByStudentIdAsync(int studentId);
        Task<IEnumerable<EnrollmentDto>> GetEnrollmentsByRoundIdAsync(int roundId);
    }
}
