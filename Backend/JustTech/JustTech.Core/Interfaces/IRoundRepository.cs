using JustTech.Core.Entities;

namespace JustTech.Core.Interfaces
{
    public interface IRoundRepository : IRepository<Round>
    {
        Task<IEnumerable<Round>> GetRoundsByCourseIdAsync(int courseId);
        Task<Round?> GetRoundWithEnrollmentsAsync(int roundId);
        Task<IEnumerable<Round>> GetActiveRoundsAsync();

    }
}
