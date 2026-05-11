using JustTech.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace JustTech.Core.Interfaces
{
    public interface IProgressRepository : IRepository<Progress>
    {
        Task<Progress?> GetProgressByStudentAndLectureAsync(int studentId, int lectureId);
        Task<IEnumerable<Progress>> GetProgressByStudentIdAsync(int studentId);
        Task<IEnumerable<Progress>> GetProgressByLectureIdAsync(int lectureId);
        Task<int> GetCompletedLecturesCountByStudentIdAsync(int studentId);
        Task<double> GetStudentProgressPercentageAsync(int studentId, int roundId);
    }
}
