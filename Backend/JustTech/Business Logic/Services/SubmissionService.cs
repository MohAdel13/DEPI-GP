using AutoMapper;
using JustTech.Core.DTOs;
using JustTech.Core.Entities;
using JustTech.Core.Interfaces;

namespace JustTech.Business.Services
{
    public class SubmissionService : ISubmissionService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public SubmissionService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IEnumerable<SubmissionDto>> GetAllAsync()
        {
            var submissions = await _unitOfWork.Submissions.GetAllAsync();
            return _mapper.Map<IEnumerable<SubmissionDto>>(submissions);
        }

        public async Task<SubmissionDto?> GetByIdAsync(int id)
        {
            var submission = await _unitOfWork.Submissions.GetByIdAsync(id);
            return submission == null ? null : _mapper.Map<SubmissionDto>(submission);
        }

        public async Task<IEnumerable<SubmissionDto>> GetSubmissionsByAssignmentIdAsync(int assignmentId)
        {
            var assignment = await _unitOfWork.Assignments.GetByIdAsync(assignmentId);
            if (assignment == null)
                return new List<SubmissionDto>();

            var submissions = await _unitOfWork.Submissions.GetSubmissionsByAssignmentIdAsync(assignmentId);
            return _mapper.Map<IEnumerable<SubmissionDto>>(submissions);
        }

        public async Task<IEnumerable<SubmissionDto>> GetSubmissionsByStudentIdAsync(int studentId)
        {
            var student = await _unitOfWork.Students.GetByIdAsync(studentId);
            if (student == null)
                return new List<SubmissionDto>();

            var submissions = await _unitOfWork.Submissions.GetSubmissionsByStudentIdAsync(studentId);
            return _mapper.Map<IEnumerable<SubmissionDto>>(submissions);
        }

        public async Task<SubmissionDto?> GetSubmissionByAssignmentAndStudentAsync(int assignmentId, int studentId)
        {
            var submission = await _unitOfWork.Submissions.GetSubmissionByAssignmentAndStudentAsync(assignmentId, studentId);
            return submission == null ? null : _mapper.Map<SubmissionDto>(submission);
        }

        public async Task<SubmissionDto?> CreateOrUpdateSubmissionAsync(CreateSubmissionDto createDto)
        {
            // Check if assignment exists
            var assignment = await _unitOfWork.Assignments.GetByIdAsync(createDto.AssignmentId);
            if (assignment == null)
                return null;

            // Check if student exists
            var student = await _unitOfWork.Students.GetByIdAsync(createDto.StudentId);
            if (student == null)
                return null;

            // Check if submission already exists
            var existingSubmission = await _unitOfWork.Submissions.GetSubmissionByAssignmentAndStudentAsync(createDto.AssignmentId, createDto.StudentId);

            if (existingSubmission != null)
            {
                existingSubmission.AnswerUrl = createDto.AnswerUrl;
                existingSubmission.SubmittedAt = DateTime.UtcNow;
                existingSubmission.Status = DetermineSubmissionStatus(assignment);
                _unitOfWork.Submissions.Update(existingSubmission);
                await _unitOfWork.SaveChangesAsync();
                return _mapper.Map<SubmissionDto>(existingSubmission);
            }

            var submission = new Submission
            {
                AssignmentId = createDto.AssignmentId,
                StudentId = createDto.StudentId,
                AnswerUrl = createDto.AnswerUrl,
                SubmittedAt = DateTime.UtcNow,
                Status = DetermineSubmissionStatus(assignment)
            };

            var created = await _unitOfWork.Submissions.AddAsync(submission);
            await _unitOfWork.SaveChangesAsync();
            return _mapper.Map<SubmissionDto>(created);
        }

        public async Task<SubmissionDto?> UpdateSubmissionAsync(int id, UpdateSubmissionDto updateDto)
        {
            var submission = await _unitOfWork.Submissions.GetByIdAsync(id);
            if (submission == null)
                return null;

            submission.AnswerUrl = updateDto.AnswerUrl;
            submission.SubmittedAt = DateTime.UtcNow;
            _unitOfWork.Submissions.Update(submission);
            await _unitOfWork.SaveChangesAsync();

            return _mapper.Map<SubmissionDto>(submission);
        }

        public async Task<SubmissionDto?> GradeSubmissionAsync(int id, GradeSubmissionDto gradeDto)
        {
            var submission = await _unitOfWork.Submissions.GetByIdAsync(id);
            if (submission == null)
                return null;

            submission.Grade = gradeDto.Grade;
            _unitOfWork.Submissions.Update(submission);
            await _unitOfWork.SaveChangesAsync();

            return _mapper.Map<SubmissionDto>(submission);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var submission = await _unitOfWork.Submissions.GetByIdAsync(id);
            if (submission == null)
                return false;

            _unitOfWork.Submissions.Delete(submission);
            await _unitOfWork.SaveChangesAsync();
            return true;
        }

        private string DetermineSubmissionStatus(Assignment assignment)
        {
            if (assignment.DueDate.HasValue && assignment.DueDate < DateTime.UtcNow)
                return "late";

            return "submitted";
        }
    }
}