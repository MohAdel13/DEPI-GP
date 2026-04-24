using JustTech.Core.Entities;

namespace JustTech.Core.Interfaces
{
    public interface IRepository<T> where T : BaseEntity
    {
       Task<T> GetByIdAsync(int id);
        Task<IEnumerable<T>> GetAllAsync();
        Task<T> AddAsync(T entity);
        Task Update(T entity);
        Task Delete(T entity);
        Task<bool> ExistsAsync(int id);

    }
}
