using JustTech.Core.DTOs;

namespace JustTech.Business.Services
{
    public interface IRoundService
    {     
        Task<IEnumerable<RoundDto>> GetAllAsync();
        Task<RoundDto?> GetByIdAsync(int id);
        Task<IEnumerable<RoundDto>> GetRoundsByCourseIdAsync(int courseId);
        Task<RoundDto> CreateAsync(CreateRoundDto createDto);
        Task<RoundDto?> UpdateAsync(int id, UpdateRoundDto updateDto);
        Task<bool> DeleteAsync(int id);
    }
}
