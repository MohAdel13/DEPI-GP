using AutoMapper;
using JustTech.Core.DTOs;
using JustTech.Core.Interfaces;
using JustTech.Core.Entities;

namespace JustTech.Business.Services
{
    public class InstructorService : IInstructorService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public InstructorService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IEnumerable<InstructorDto>> GetAllAsync()
        {
            var instructors = await _unitOfWork.Instructors.GetAllAsync();
            return _mapper.Map<IEnumerable<InstructorDto>>(instructors);
        }
        public async Task<InstructorDto?> GetByIdAsync(int id)
        {
            var instructor = await _unitOfWork.Instructors.GetByIdAsync(id);
            return instructor == null ? null : _mapper.Map<InstructorDto>(instructor);
        }

        public async Task<InstructorDto> CreateAsync(CreateInstructorDto createDto)
        {
            // Check if email already exists
            var existingInstructor = await _unitOfWork.Instructors.GetByEmailAsync(createDto.Email);
            if (existingInstructor != null)
                throw new Exception("Instructor with this email already exists");

            var instructor = _mapper.Map<Instructor>(createDto);
            instructor.Password = BCrypt.Net.BCrypt.HashPassword(createDto.Password);

            var created = await _unitOfWork.Instructors.AddAsync(instructor);
            await _unitOfWork.SaveChangesAsync();

            return _mapper.Map<InstructorDto>(created);
        }
        public async Task<InstructorDto?> UpdateAsync(int id, UpdateInstructorDto updateDto)
        {
            var instructor = await _unitOfWork.Instructors.GetByIdAsync(id);
            if (instructor == null)
                return null;

            _mapper.Map(updateDto, instructor);
            _unitOfWork.Instructors.Update(instructor);
            await _unitOfWork.SaveChangesAsync();

            return _mapper.Map<InstructorDto>(instructor);
        }



        public async Task<bool> DeleteAsync(int id)
        {
            var instructor = await _unitOfWork.Instructors.GetByIdAsync(id);
            if (instructor == null)
                return false;

            _unitOfWork.Instructors.Delete(instructor);
            await _unitOfWork.SaveChangesAsync();
            return true;
        }



        public async Task<InstructorDto?> GetByEmailAsync(string email)
        {
            var instructor = await _unitOfWork.Instructors.GetByEmailAsync(email);
            return instructor == null ? null : _mapper.Map<InstructorDto>(instructor);
        }

        public async Task<IEnumerable<InstructorDto>> GetInstructorsByProfessionAsync(string profession)
        {
            var instructors = await _unitOfWork.Instructors.GetInstructorsByProfessionAsync(profession);
            return _mapper.Map<IEnumerable<InstructorDto>>(instructors);
        }
        public async Task<IEnumerable<InstructorDto>> GetInstructorsByCityAsync(string city)
        {
            var instructors = await _unitOfWork.Instructors.GetInstructorsByCityAsync(city);
            return _mapper.Map<IEnumerable<InstructorDto>>(instructors);
        }
    }
}
