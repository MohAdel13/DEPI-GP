using JustTech.Core.Entities;

namespace JustTech.Core.Interfaces
{
    public interface ICourseRepository : IRepository<Course>
    {
        // Data Access Layer (ICourseRepository) => Talks to database only
        Task<IEnumerable<Course>> GetAllCoursesAsync();
        Task<Course?> GetCourseWithRoundsAsync(int courseId);
    }
}
