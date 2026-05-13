using JustTech.Core.DTOs;
namespace Business.Logic.Services
{
    public interface ICourseService
    {
        Task<IEnumerable<CourseDto>> GetAllAsync();
        Task<CourseDto?> GetByIdAsync(int id);
        Task<CourseDto> CreateAsync(CreateCourseDto createDto);
        Task<CourseDto?> UpdateAsync(int id, UpdateCourseDto updateDto);
        Task<bool> DeleteAsync(int id);
    }
}
