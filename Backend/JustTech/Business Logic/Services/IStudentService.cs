using JustTech.Core.DTOs;

namespace JustTech.Business.Services
{
    public interface IStudentService
    {
        Task<IEnumerable<StudentDto>> GetAllAsync();
        Task<StudentDto?> GetByIdAsync(int id);
        Task<StudentDto> CreateAsync(CreateStudentDto createDto);
        Task<StudentDto?> UpdateAsync(int id, UpdateStudentDto updateDto);
        Task<bool> DeleteAsync(int id);
        Task<IEnumerable<StudentDto>> GetStudentsByStatusAsync(string status);
        Task<IEnumerable<StudentDto>> GetStudentsByCityAsync(string city);
    }
}
