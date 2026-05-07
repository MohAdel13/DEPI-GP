using JustTech.Core.DTOs;

namespace JustTech.Business.Services
{
    public interface IRoundService
    {
        Task<IEnumerable<RoundDto>> GetAllAsync();
        Task<RoundDto> GetByIdAsync(int id);
        Task<RoundDto> CreateAsync(CreateRoundDto createDto);
        Task<RoundDto> UpdateAsync(int id, UpdateRoundDto updateDto);
        Task DeleteAsync(int id);
        Task<IEnumerable<RoundDto>> GetRoundByCourseIdAsync(int courseId);
    }
}
