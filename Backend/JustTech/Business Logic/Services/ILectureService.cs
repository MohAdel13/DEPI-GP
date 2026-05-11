using JustTech.Core.DTOs;

namespace JustTech.Business.Services
{
    public interface ILectureService
    {
        Task<IEnumerable<LectureDto>> GetAllAsync();
        Task<LectureDto> GetByIdAsync(int id);
        Task<IEnumerable<LectureDto>> GetLecturesByRoundIdAsync(int roundId);
        Task<LectureDto> CreateAsync(CreateLectureDto createDto);
        Task<LectureDto> UpdateAsync(int id, UpdateLectureDto updateDto);
        Task DeleteAsync(int id);
    }
}
