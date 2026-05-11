using JustTech.Core.Entities;

namespace JustTech.Core.Interfaces
{
    public interface ILectureRepository : IRepository<Lecture>
    {
        Task<IEnumerable<Lecture>> GetLecturesByRoundIdAsync(int roundId);
        Task<Lecture?> GetLectureWithMaterialsAsync(int lectureId);
    }
}
