using JustTech.Core.DTOs;
namespace Business.Logic.Services
{
    public interface ICourseService
    {
        Task<IEnumerable<CourseDto>> GetAllAsync();
        Task<CourseDto> GetByIdAsync(int id);
        Task<CourseDto> CreateAsync(CreateCourseDto courseDto);
        Task<CourseDto> UpdateAsync(int id ,UpdateCourseDto courseDto);
        Task DeleteAsync(int id);
    }
}
