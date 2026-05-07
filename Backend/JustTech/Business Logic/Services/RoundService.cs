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

        public async Task<RoundDto> GetByIdAsync(int id)
        {
            var round = await _unitOfWork.Rounds.GetByIdAsync(id);
            if (round == null)
                throw new Exception($"Round with ID {id} not found");

            return _mapper.Map<RoundDto>(round);
        }

        public async Task<RoundDto> CreateAsync(CreateRoundDto createDto)
        {
            // Validate course exists
            var course = await _unitOfWork.Courses.GetByIdAsync(createDto.CourseId);
            if (course == null)
                throw new Exception($"Course With ID {createDto.CourseId} not found");

            // Validate instructor exist
            /*
            var instractor = await _unitOfWork.Instructors.GetByIdAsync(createDto.InstructorId);  // I did not implement Instructor yet
            if (instractor == null)
                throw new Exception($"Instructor with ID {createDto.InstructorId} not found");
            */

            var round = _mapper.Map<Round>(createDto);
            var created = await _unitOfWork.Rounds.AddAsync(round);
            await _unitOfWork.SaveChangesAsync();

            return _mapper.Map<RoundDto>(created);
        }


        public async Task<RoundDto> UpdateAsync(int id, UpdateRoundDto updateDto)
        {
            var round = await _unitOfWork.Rounds.GetByIdAsync(id);
            if (round == null)
                throw new Exception($"Round with ID {id} not found");

            _mapper.Map(updateDto, round);
            _unitOfWork.Rounds.Update(round);
            await _unitOfWork.SaveChangesAsync();

            return _mapper.Map<RoundDto>(round);
        }


        public async Task DeleteAsync(int id)
        {
            var round = await _unitOfWork.Rounds.GetByIdAsync(id);
            if (round == null)
                throw new Exception($"Round with ID {id} not found");

            _unitOfWork.Rounds.Delete(round);
            await _unitOfWork.SaveChangesAsync();

        }
        

        public async Task<IEnumerable<RoundDto>> GetRoundByCourseIdAsync(int courseId)
        {
            var rounds = await _unitOfWork.Rounds.GetRoundsByCourseIdAsync(courseId);
            return _mapper.Map<IEnumerable<RoundDto>>(rounds);
        }

      
    }
}
