using JustTech.Core.Entities;

namespace JustTech.Core.Interfaces
{
    public interface IInstructorRepository : IRepository<Instructor>
    {
        Task<Instructor?> GetByEmailAsync(string email);
        Task<IEnumerable<Instructor>> GetInstructorsByProfessionAsync(string profession);
        Task<IEnumerable<Instructor>> GetInstructorsByCityAsync(string city);
    }
}
