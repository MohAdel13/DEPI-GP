using AutoMapper;
using JustTech.Core.DTOs;
using JustTech.Core.Entities;
using JustTech.Core.Interfaces;

namespace JustTech.Business.Services
{
    public class LectureService : ILectureService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public LectureService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IEnumerable<LectureDto>> GetAllAsync()
        {
            var lectures = await _unitOfWork.Lectures.GetAllAsync();
            return _mapper.Map<IEnumerable<LectureDto>>(lectures);
        }
        public async Task<LectureDto> GetByIdAsync(int id)
        {
            var lecture = await _unitOfWork.Lectures.GetByIdAsync(id);
            if (lecture == null)
                throw new Exception($"Lecture with ID {id} not found");

            return _mapper.Map<LectureDto>(lecture);
        }

        public async Task<IEnumerable<LectureDto>> GetLecturesByRoundIdAsync(int roundId)
        {
            var round = await _unitOfWork.Rounds.GetByIdAsync(roundId);
            if (round == null)
                throw new Exception($"Round with ID {roundId} not found");

            var lectures = await _unitOfWork.Lectures.GetLecturesByRoundIdAsync(roundId);
            return _mapper.Map<IEnumerable<LectureDto>>(lectures);
        }
        public async Task<LectureDto> CreateAsync(CreateLectureDto createDto)
        {
            // Check if round exists
            var round = await _unitOfWork.Rounds.GetByIdAsync(createDto.RoundId);
            if (round == null)
                throw new Exception($"Round with ID {createDto.RoundId} not found");

            var lecture = _mapper.Map<Lecture>(createDto);
            var created = await _unitOfWork.Lectures.AddAsync(lecture);
            await _unitOfWork.SaveChangesAsync();

            return _mapper.Map<LectureDto>(created);
        }
        public async Task<LectureDto> UpdateAsync(int id, UpdateLectureDto updateDto)
        {
            var lecture = await _unitOfWork.Lectures.GetByIdAsync(id);
            if (lecture == null)
                throw new Exception($"Lecture with ID {id} not found");

            _mapper.Map(updateDto, lecture);
            _unitOfWork.Lectures.Update(lecture);
            await _unitOfWork.SaveChangesAsync();

            return _mapper.Map<LectureDto>(lecture);
        }
        public async Task DeleteAsync(int id)
        {
            var lecture = await _unitOfWork.Lectures.GetByIdAsync(id);
            if (lecture == null)
                throw new Exception($"Lecture with ID {id} not found");

            _unitOfWork.Lectures.Delete(lecture);
            await _unitOfWork.SaveChangesAsync();
        }

    }
}
