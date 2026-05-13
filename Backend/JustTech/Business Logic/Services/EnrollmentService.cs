using AutoMapper;
using JustTech.Core.DTOs;
using JustTech.Core.Entities;
using JustTech.Core.Interfaces;

namespace JustTech.Business.Services
{
    public class EnrollmentService : IEnrollmentService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public EnrollmentService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;   
        }

        public async Task<IEnumerable<EnrollmentDto>> GetAllAsync()
        {
            var enrollments = await _unitOfWork.Enrollments.GetAllAsync();
            return _mapper.Map<IEnumerable<EnrollmentDto>>(enrollments);
        }

        public async Task<EnrollmentDto?> GetByIdAsync(int id)
        {
            var enrollment = await _unitOfWork.Enrollments.GetByIdAsync(id);
            return enrollment == null ? null : _mapper.Map<EnrollmentDto>(enrollment);
        }
        public async Task<EnrollmentDto?> EnrollStudentAsync(CreateEnrollmentDto createDto)
        {
            var student = await _unitOfWork.Students.GetByIdAsync(createDto.StudentId);
            if (student == null)
                return null;

            var round = await _unitOfWork.Rounds.GetByIdAsync(createDto.RoundId);
            if (round == null)
                return null;

            var alreadyEnrolled = await _unitOfWork.Enrollments.IsStudentEnrolledAsync(createDto.StudentId, createDto.RoundId);
            if (alreadyEnrolled)
                return null;

            if (round.Status != "in progress")
                return null;

            var enrollment = _mapper.Map<Enrollment>(createDto);
            enrollment.Status = "active";
            enrollment.EnrolledAt = DateTime.UtcNow;

            var created = await _unitOfWork.Enrollments.AddAsync(enrollment);
            await _unitOfWork.SaveChangesAsync();

            return _mapper.Map<EnrollmentDto>(created);
        }


        public async Task<EnrollmentDto?> UpdateEnrollmentStatusAsync(int id, UpdateEnrollmentStatusDto updateDto)
        {
            var enrollment = await _unitOfWork.Enrollments.GetByIdAsync(id);
            if (enrollment == null)
                return null;

            enrollment.Status = updateDto.Status;
            _unitOfWork.Enrollments.Update(enrollment);
            await _unitOfWork.SaveChangesAsync();

            return _mapper.Map<EnrollmentDto>(enrollment);
        }


        public async Task<bool> DeleteAsync(int id)
        {
            var enrollment = await _unitOfWork.Enrollments.GetByIdAsync(id);
            if (enrollment == null)
                return false;

            _unitOfWork.Enrollments.Delete(enrollment);
            await _unitOfWork.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<EnrollmentDto>> GetEnrollmentsByStudentIdAsync(int studentId)
        {
            var enrollments = await _unitOfWork.Enrollments.GetEnrollmentsByStudentIdAsync(studentId);

            return _mapper.Map <IEnumerable<EnrollmentDto>>(enrollments);
        }


        public async Task<IEnumerable<EnrollmentDto>> GetEnrollmentsByRoundIdAsync(int roundId)
        {
            var enrollments = await _unitOfWork.Enrollments.GetEnrollmentsByRoundIdAsync(roundId);

            return _mapper.Map<IEnumerable<EnrollmentDto>>(enrollments);
        }

        

       
    }
}
