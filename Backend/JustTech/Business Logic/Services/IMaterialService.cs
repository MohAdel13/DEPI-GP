using JustTech.Core.DTOs;

namespace JustTech.Business.Services
{
    public interface IMaterialService
    {
        Task<IEnumerable<MaterialDto>> GetAllAsync();
        Task<MaterialDto?> GetByIdAsync(int id);
        Task<IEnumerable<MaterialDto>> GetMaterialsByLectureIdAsync(int lectureId);
        Task<MaterialDto> CreateAsync(CreateMaterialDto createDto);
        Task<MaterialDto?> UpdateAsync(int id, UpdateMaterialDto updateDto);
        Task<bool> DeleteAsync(int id);
    }
}
