using JustTech.Core.DTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace JustTech.Business.Services
{
    public interface ISubmissionService
    {
        Task<IEnumerable<SubmissionDto>> GetAllAsync();
        Task<SubmissionDto?> GetByIdAsync(int id);
        Task<IEnumerable<SubmissionDto>> GetSubmissionsByAssignmentIdAsync(int assignmentId);
        Task<IEnumerable<SubmissionDto>> GetSubmissionsByStudentIdAsync(int studentId);
        Task<SubmissionDto?> GetSubmissionByAssignmentAndStudentAsync(int assignmentId, int studentId);
        Task<SubmissionDto?> CreateOrUpdateSubmissionAsync(CreateSubmissionDto createDto);  // Changed to nullable
        Task<SubmissionDto?> UpdateSubmissionAsync(int id, UpdateSubmissionDto updateDto);
        Task<SubmissionDto?> GradeSubmissionAsync(int id, GradeSubmissionDto gradeDto);
        Task<bool> DeleteAsync(int id);
    }
}
