using JustTech.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace JustTech.Core.Interfaces
{
    public interface ISubmissionRepository : IRepository<Submission>
    {
        Task<IEnumerable<Submission>> GetSubmissionsByAssignmentIdAsync(int assignmentId);
        Task<IEnumerable<Submission>> GetSubmissionsByStudentIdAsync(int studentId);
        Task<Submission?> GetSubmissionByAssignmentAndStudentAsync(int assignmentId, int studentId);
        Task<IEnumerable<Submission>> GetSubmissionsByStatusAsync(string status);
    }
}
