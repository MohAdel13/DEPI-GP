using JustTech.Core.Entities;

namespace JustTech.Core.Interfaces
{
    public interface IStudentRepository : IRepository<Student>
    {
        Task<Student> GetByEmailAsync(string email);
        Task<IEnumerable<Student>> GetStudentsByStatusAsync(string status); 
        Task<IEnumerable<Student>> GetStudentsByCityAsync(string city);    
    }
}
