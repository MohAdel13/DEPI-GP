using AutoMapper;
using JustTech.Core.DTOs;
using JustTech.Core.Interfaces;

namespace JustTech.Business.Services
{
    public class ProgressService : IProgressService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ProgressService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IEnumerable<ProgressDto>> GetAllAsync()
        {
            var progresses = await _unitOfWork.Progresses.GetAllAsync();
            return _mapper.Map<IEnumerable<ProgressDto>>(progresses);
        }

        public async Task<ProgressDto?> GetByIdAsync(int id)
        {
            var progress = await _unitOfWork.Progresses.GetByIdAsync(id);
            return progress == null ? null : _mapper.Map<ProgressDto>(progress);
        }

        public async Task<ProgressDto?> GetProgressByStudentAndLectureAsync(int studentId, int lectureId)
        {
            var progress = await _unitOfWork.Progresses.GetProgressByStudentAndLectureAsync(studentId, lectureId);
            return progress == null ? null : _mapper.Map<ProgressDto>(progress);
        }

        public async Task<IEnumerable<ProgressDto>> GetProgressByStudentIdAsync(int studentId)
        {
            var progresses = await _unitOfWork.Progresses.GetProgressByStudentIdAsync(studentId);
            return _mapper.Map<IEnumerable<ProgressDto>>(progresses);
        }

        public async Task<IEnumerable<ProgressDto>> GetProgressByLectureIdAsync(int lectureId)
        {
            var progresses = await _unitOfWork.Progresses.GetProgressByLectureIdAsync(lectureId);
            return _mapper.Map<IEnumerable<ProgressDto>>(progresses);
        }

        public async Task<ProgressDto?> CreateOrUpdateProgressAsync(CreateProgressDto createDto)
        {
            var student = await _unitOfWork.Students.GetByIdAsync(createDto.StudentId);
            if (student == null)
                return null;

            var lecture = await _unitOfWork.Lectures.GetByIdAsync(createDto.LectureId);
            if (lecture == null)
                return null;

            var existingProgress = await _unitOfWork.Progresses.GetProgressByStudentAndLectureAsync(createDto.StudentId, createDto.LectureId);

            if (existingProgress != null)
            {
                existingProgress.LastWatchedAt = DateTime.UtcNow;
                _unitOfWork.Progresses.Update(existingProgress);
                await _unitOfWork.SaveChangesAsync();
                return _mapper.Map<ProgressDto>(existingProgress);
            }

            var progress = new JustTech.Core.Entities.Progress
            {
                StudentId = createDto.StudentId,
                LectureId = createDto.LectureId,
                IsCompleted = false,
                LastWatchedAt = DateTime.UtcNow
            };

            var created = await _unitOfWork.Progresses.AddAsync(progress);
            await _unitOfWork.SaveChangesAsync();

            return _mapper.Map<ProgressDto>(created);
        }

        public async Task<ProgressDto?> MarkLectureCompletedAsync(int studentId, int lectureId)
        {
            var student = await _unitOfWork.Students.GetByIdAsync(studentId);
            if (student == null)
                return null;

            var lecture = await _unitOfWork.Lectures.GetByIdAsync(lectureId);
            if (lecture == null)
                return null;

            var progress = await _unitOfWork.Progresses.GetProgressByStudentAndLectureAsync(studentId, lectureId);

            if (progress == null)
            {
                progress = new JustTech.Core.Entities.Progress
                {
                    StudentId = studentId,
                    LectureId = lectureId,
                    IsCompleted = true,
                    CompletedAt = DateTime.UtcNow,
                    LastWatchedAt = DateTime.UtcNow
                };
                var created = await _unitOfWork.Progresses.AddAsync(progress);
                await _unitOfWork.SaveChangesAsync();
                return _mapper.Map<ProgressDto>(created);
            }

            progress.IsCompleted = true;
            progress.CompletedAt = DateTime.UtcNow;
            progress.LastWatchedAt = DateTime.UtcNow;
            _unitOfWork.Progresses.Update(progress);
            await _unitOfWork.SaveChangesAsync();

            return _mapper.Map<ProgressDto>(progress);
        }
        public async Task<ProgressDto?> UpdateProgressAsync(int id, UpdateProgressDto updateDto)
        {
            var progress = await _unitOfWork.Progresses.GetByIdAsync(id);
            if (progress == null)
                return null;

            progress.IsCompleted = updateDto.IsCompleted;
            if (updateDto.IsCompleted)
                progress.CompletedAt = DateTime.UtcNow;

            _unitOfWork.Progresses.Update(progress);
            await _unitOfWork.SaveChangesAsync();

            return _mapper.Map<ProgressDto>(progress);
        }
        public async Task<bool> DeleteAsync(int id)
        {
            var progress = await _unitOfWork.Progresses.GetByIdAsync(id);
            if (progress == null)
                return false;

            _unitOfWork.Progresses.Delete(progress);
            await _unitOfWork.SaveChangesAsync();
            return true;
        }
        public async Task<int> GetCompletedLecturesCountAsync(int studentId)
        {
            return await _unitOfWork.Progresses.GetCompletedLecturesCountByStudentIdAsync(studentId);
        }

        public async Task<double> GetStudentProgressPercentageAsync(int studentId, int roundId)
        {
            return await _unitOfWork.Progresses.GetStudentProgressPercentageAsync(studentId, roundId);
        }

    }
}
