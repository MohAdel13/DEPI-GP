using JustTech.Core.DTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace JustTech.Business.Services
{
    public interface IProgressService
    {
        Task<IEnumerable<ProgressDto>> GetAllAsync();
        Task<ProgressDto> GetByIdAsync(int id);
        Task<ProgressDto> GetProgressByStudentAndLectureAsync(int studentId, int lectureId);
        Task<IEnumerable<ProgressDto>> GetProgressByStudentIdAsync(int studentId);
        Task<IEnumerable<ProgressDto>> GetProgressByLectureIdAsync(int lectureId);
        Task<ProgressDto> CreateOrUpdateProgressAsync(CreateProgressDto createDto);
        Task<ProgressDto> MarkLectureCompletedAsync(int studentId, int lectureId);
        Task<ProgressDto> UpdateProgressAsync(int id, UpdateProgressDto updateDto);
        Task DeleteAsync(int id);
        Task<int> GetCompletedLecturesCountAsync(int studentId);
        Task<double> GetStudentProgressPercentageAsync(int studentId, int roundId);
    }
}
