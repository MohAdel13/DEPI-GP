using JustTech.Core.Entities;

namespace JustTech.Core.Interfaces
{
    public interface IEnrollmentRepository : IRepository<Enrollment>
    {
        Task<IEnumerable<Enrollment>> GetEnrollmentsByStudentIdAsync(int studentId);
        Task<IEnumerable<Enrollment>> GetEnrollmentsByRoundIdAsync(int roundId);
        Task<Enrollment?> GetEnrollmentByStudentAndRoundAsync(int studentId, int roundId);
        Task<bool> IsStudentEnrolledAsync(int studentId, int roundId);
    }
}
