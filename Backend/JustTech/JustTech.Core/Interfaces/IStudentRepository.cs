using JustTech.Core.Entities;

namespace JustTech.Core.Interfaces
{
    public interface IStudentRepository : IRepository<Student>
    {
        Task<Student> GetByEmailAsync(string email);
    }
}
