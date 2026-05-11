using JustTech.Core.DTOs;

namespace JustTech.Business.Services
{
    public interface IInstructorService
    {
        Task<IEnumerable<InstructorDto>> GetAllAsync();
        Task<InstructorDto> GetByIdAsync(int id);
        Task<InstructorDto> CreateAsync(CreateInstructorDto createDto);
        Task<InstructorDto> UpdateAsync(int id, UpdateInstructorDto updateDto);
        Task DeleteAsync(int id);
        Task<InstructorDto> GetByEmailAsync(string email);
        Task<IEnumerable<InstructorDto>> GetInstructorsByProfessionAsync(string profession);
        Task<IEnumerable<InstructorDto>> GetInstructorsByCityAsync(string city);
    }
}
