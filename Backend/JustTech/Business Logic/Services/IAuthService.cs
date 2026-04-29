using JustTech.Core.DTOs;
namespace JustTech.Business.Services
{
    public interface IAuthService
    {
        Task<StudentAuthResponseDto> RegisterAsync(StudentRegisterDto registerDto);
        Task<StudentAuthResponseDto> LoginAsync(StudentLoginDto loginDto);
        Task<bool> EmailExistsAsync(string email);
    }
}
