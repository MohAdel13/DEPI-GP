using AutoMapper;
using JustTech.Core.DTOs;
using JustTech.Core.Entities;
using JustTech.Core.Interfaces;

namespace JustTech.Business.Services
{
    public class AssignmentService : IAssignmentService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public AssignmentService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IEnumerable<AssignmentDto>> GetAllAsync()
        {
            var assignments = await _unitOfWork.Assignments.GetAllAsync();
            return _mapper.Map<IEnumerable<AssignmentDto>>(assignments);
        }

        public async Task<AssignmentDto?> GetByIdAsync(int id)
        {
            var assignment = await _unitOfWork.Assignments.GetByIdAsync(id);
            return assignment == null ? null : _mapper.Map<AssignmentDto>(assignment);
        }

        public async Task<IEnumerable<AssignmentDto>> GetAssignmentsByRoundIdAsync(int roundId)
        {
            var round = await _unitOfWork.Rounds.GetByIdAsync(roundId);
            if (round == null)
                return new List<AssignmentDto>();

            var assignments = await _unitOfWork.Assignments.GetAssignmentsByRoundIdAsync(roundId);
            return _mapper.Map<IEnumerable<AssignmentDto>>(assignments);
        }

        public async Task<AssignmentDto> CreateAsync(CreateAssignmentDto createDto)
        {
            // Check if round exists
            var round = await _unitOfWork.Rounds.GetByIdAsync(createDto.RoundId);
            if (round == null)
                throw new Exception($"Round with ID {createDto.RoundId} not found");

            var assignment = _mapper.Map<Assignment>(createDto);
            var created = await _unitOfWork.Assignments.AddAsync(assignment);
            await _unitOfWork.SaveChangesAsync();

            return _mapper.Map<AssignmentDto>(created);
        }
        public async Task<AssignmentDto?> UpdateAsync(int id, UpdateAssignmentDto updateDto)
        {
            var assignment = await _unitOfWork.Assignments.GetByIdAsync(id);
            if (assignment == null)
                return null;

            _mapper.Map(updateDto, assignment);
            _unitOfWork.Assignments.Update(assignment);
            await _unitOfWork.SaveChangesAsync();

            return _mapper.Map<AssignmentDto>(assignment);
        }
        public async Task<bool> DeleteAsync(int id)
        {
            var assignment = await _unitOfWork.Assignments.GetByIdAsync(id);
            if (assignment == null)
                return false;

            _unitOfWork.Assignments.Delete(assignment);
            await _unitOfWork.SaveChangesAsync();
            return true;
        }

    }
}
