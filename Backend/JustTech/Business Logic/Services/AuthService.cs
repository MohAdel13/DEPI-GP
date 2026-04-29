using AutoMapper;
using JustTech.Core.DTOs;
using JustTech.Core.Interfaces;
using Microsoft.Extensions.Configuration;
using JustTech.Core.Entities;
using System.Text;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using BCrypt.Net;

namespace JustTech.Business.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;     // install Package Microsoft.Extensions.Configuration.Abstractions

        public AuthService(IUnitOfWork unitOfWork, IMapper mapper, IConfiguration configuration)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _configuration = configuration;
        }
        public async Task<StudentAuthResponseDto> RegisterAsync(StudentRegisterDto registerDto)
        {
            // Check If email already exists
            var emailExists = await EmailExistsAsync(registerDto.Email);
            if (emailExists)
                throw new Exception("Email already registered");

            // Map Dto To Entity
            var student = _mapper.Map<Student>(registerDto);


            // Hash Password
            student.Password = HashPassword(registerDto.Password);

            // Add Student
            var created = await _unitOfWork.Students.AddAsync(student);
            await _unitOfWork.SaveChangesAsync();

            // Generate token
            var token = GenerateJwtToken(created);

            return new StudentAuthResponseDto
            { 
                Id = created.Id,
                Name = created.Name,
                Email = created.Email,
                Token = token, //  ← Add this line (use the token variable from line 44)
                TokenExpiration = DateTime.UtcNow.AddHours(24)
            };
        }

        private string HashPassword(string password)
        {
            // Temporary - will implement properly in step 10
            return BCrypt.Net.BCrypt.HashPassword(password);
        }
        private bool VerifyPassword(string password, string hash)
        {
            // Temporary - will implement properly in step 10
            return BCrypt.Net.BCrypt.Verify(password, hash);
        }
        private string GenerateJwtToken(Student student)
        {
            var jwtSettings = _configuration.GetSection("JwtSettings");
            var key = Encoding.UTF8.GetBytes(jwtSettings["SecretKey"]);

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, student.Id.ToString()),
                new Claim(ClaimTypes.Email, student.Email),
                new Claim(ClaimTypes.Name, student.Name),
                new Claim(ClaimTypes.Role, "Student")
            };

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddHours(24),
                Issuer = jwtSettings["Issuer"],
                Audience = jwtSettings["Audience"],
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        public async Task<bool> EmailExistsAsync(string email)
        {
            var student = await _unitOfWork.Students.GetByEmailAsync(email);
            return student != null;
        }

        public async Task<StudentAuthResponseDto> LoginAsync(StudentLoginDto loginDto)
        {
            var student = await _unitOfWork.Students.GetByEmailAsync(loginDto.Email);
            if (student == null)
                throw new Exception("Inavlid email or password");

            // verify password
            if (!VerifyPassword(loginDto.Password, student.Password))
                throw new Exception("Invalid email or password");

            var token = GenerateJwtToken(student);

            return new StudentAuthResponseDto
            {
                Id = student.Id,
                Name = student.Name,
                Email = student.Email,
                Token = token,
                TokenExpiration = DateTime.UtcNow.AddHours(24)
            };
        }
    }
}
