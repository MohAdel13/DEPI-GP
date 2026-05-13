using AutoMapper;
using JustTech.Core.DTOs;
using JustTech.Core.Entities;
using JustTech.Core.Interfaces;

namespace JustTech.Business.Services
{
    public class StudentService : IStudentService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public StudentService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IEnumerable<StudentDto>> GetAllAsync()
        {
            var students = await _unitOfWork.Students.GetAllAsync();
            return _mapper.Map<IEnumerable<StudentDto>>(students);
        }

        public async Task<StudentDto?> GetByIdAsync(int id)
        {
            var student = await _unitOfWork.Students.GetByIdAsync(id);
            return student == null ? null : _mapper.Map<StudentDto>(student);
        }

        public async Task<StudentDto> CreateAsync(CreateStudentDto createDto)
        {
            // Check if email already exists
            var existingStudent = await _unitOfWork.Students.GetByEmailAsync(createDto.Email);
            if (existingStudent != null)
                throw new Exception("Student with this email is already exists");

            var student = _mapper.Map<Student>(createDto);
            student.Password = BCrypt.Net.BCrypt.HashPassword(createDto.Password);

            var created = await _unitOfWork.Students.AddAsync(student);
            await _unitOfWork.SaveChangesAsync();

            return _mapper.Map<StudentDto>(created);
        }

        public async Task<StudentDto?> UpdateAsync(int id, UpdateStudentDto updateDto)
        {
            var student = await _unitOfWork.Students.GetByIdAsync(id);
            if (student == null)
                return null;

            _mapper.Map(updateDto, student);
            _unitOfWork.Students.Update(student);
            await _unitOfWork.SaveChangesAsync();

            return _mapper.Map<StudentDto>(student);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var student = await _unitOfWork.Students.GetByIdAsync(id);
            if (student == null)
                return false;

            _unitOfWork.Students.Delete(student);
            await _unitOfWork.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<StudentDto>> GetStudentsByStatusAsync(string status)
        {
            var students = await _unitOfWork.Students.GetStudentsByStatusAsync(status);
            return _mapper.Map<IEnumerable<StudentDto>>(students);
        }

        public async Task<IEnumerable<StudentDto>> GetStudentsByCityAsync(string city)
        {
            var students = await _unitOfWork.Students.GetStudentsByCityAsync(city);
            return _mapper.Map<IEnumerable<StudentDto>>(students);
        }
    }
}
