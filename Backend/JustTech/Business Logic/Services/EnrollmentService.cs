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
            return _mapper.Map <IEnumerable<EnrollmentDto>>(enrollments);
        }

        public async Task<EnrollmentDto> GetByIdAsync(int id)
        {
            var enrollment = await _unitOfWork.Enrollments.GetByIdAsync(id);
            if (enrollment == null)
                throw new Exception($"Enrollment with ID {id} not found");

            return _mapper.Map<EnrollmentDto>(enrollment);
        }
        public async Task<EnrollmentDto> EnrollStudentAsync(CreateEnrollmentDto createDto)
        {
            // Check if student exist
            var student = await _unitOfWork.Students.GetByIdAsync(createDto.StudentId);
            if (student == null)
                throw new Exception($"Student with ID {createDto.StudentId} not found");

            // Check round exist
            var round = await _unitOfWork.Rounds.GetByIdAsync(createDto.RoundId);
            if (round == null)
                throw new Exception($"Round with ID {createDto.RoundId} not found");

            // Check if already enrolled
            var alreadyEnrolled = await _unitOfWork.Enrollments
                .IsStudentEnrolledAsync(createDto.StudentId, createDto.RoundId);
            if (alreadyEnrolled)
                throw new Exception("Student is already enrolled in this round");

            // Check if round is active for enrollment
            if (round.Status != "is progress")
                throw new Exception($"Can't Enroll in round with status: {round.Status}");

            var enrollment = _mapper.Map<Enrollment>(createDto);
            enrollment.Status = "active";
            enrollment.EnrolledAt = DateTime.UtcNow;

            var created = await _unitOfWork.Enrollments.AddAsync(enrollment);
            await _unitOfWork.SaveChangesAsync();


            return _mapper.Map<EnrollmentDto>(created);

        }


        public async Task<EnrollmentDto> UpdateEnrollmentStatusAsync(int id, UpdateEnrollmentStatusDto updateDto)
        {
            var enrollment = await _unitOfWork.Enrollments.GetByIdAsync(id);
            if (enrollment == null)
                throw new Exception($"Enrollment with ID {id} not found");

            enrollment.Status = updateDto.Status;
            _unitOfWork.Enrollments.Update(enrollment);
            await _unitOfWork.SaveChangesAsync();

            return _mapper.Map<EnrollmentDto>(enrollment);
        }


        public async Task DeleteAsync(int id)
        {
            var enrollemnt = await _unitOfWork.Enrollments.GetByIdAsync(id);
            if (enrollemnt == null)
                throw new Exception($"Enrollment with ID {id} not found");

            _unitOfWork.Enrollments.Delete(enrollemnt);
            await _unitOfWork.SaveChangesAsync();
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
