using AutoMapper;
using JustTech.Core.DTOs;
using JustTech.Core.Entities;
using JustTech.Core.Interfaces;

namespace JustTech.Business.Services
{
    public class RoundService : IRoundService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public RoundService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task<IEnumerable<RoundDto>> GetAllAsync()
        {
            var rounds = await _unitOfWork.Rounds.GetAllAsync();
            return _mapper.Map<IEnumerable<RoundDto>>(rounds);
        }

        public async Task<RoundDto?> GetByIdAsync(int id)
        {
            var round = await _unitOfWork.Rounds.GetByIdAsync(id);
            return round == null ? null : _mapper.Map<RoundDto>(round);
        }

        public async Task<RoundDto> CreateAsync(CreateRoundDto createDto)
        {
            // Validate course exists
            var course = await _unitOfWork.Courses.GetByIdAsync(createDto.CourseId);
            if (course == null)
                return null;  // Changed from exception to return null

            // Validate instructor exists (now uncommented)
            var instructor = await _unitOfWork.Instructors.GetByIdAsync(createDto.InstructorId);
            if (instructor == null)
                return null;  // Return null if instructor not found

            var round = _mapper.Map<Round>(createDto);
            var created = await _unitOfWork.Rounds.AddAsync(round);
            await _unitOfWork.SaveChangesAsync();

            return _mapper.Map<RoundDto>(created);
        }


        public async Task<RoundDto?> UpdateAsync(int id, UpdateRoundDto updateDto)
        {
            var round = await _unitOfWork.Rounds.GetByIdAsync(id);
            if (round == null)
                return null;

            _mapper.Map(updateDto, round);
            _unitOfWork.Rounds.Update(round);
            await _unitOfWork.SaveChangesAsync();

            return _mapper.Map<RoundDto>(round);
        }


        public async Task<bool> DeleteAsync(int id)
        {
            var round = await _unitOfWork.Rounds.GetByIdAsync(id);
            if (round == null)
                return false;

            _unitOfWork.Rounds.Delete(round);
            await _unitOfWork.SaveChangesAsync();
            return true;
        }


        public async Task<IEnumerable<RoundDto>> GetRoundsByCourseIdAsync(int courseId)
        {
            var rounds = await _unitOfWork.Rounds.GetRoundsByCourseIdAsync(courseId);
            return _mapper.Map<IEnumerable<RoundDto>>(rounds);
        }

      
    }
}
