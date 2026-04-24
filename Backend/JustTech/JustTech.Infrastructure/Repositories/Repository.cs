using JustTech.Core.Entities;
using JustTech.Core.Interfaces;
using JustTech.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace JustTech.Infrastructure.Repositories
{
    public class Repository<T> : IRepository<T> where T : BaseEntity
    {

        protected readonly AppDbContext _context;
        private readonly DbSet<T> _dbSet;

        public Repository(AppDbContext context)
        {
            _context = context;
            _dbSet = context.Set<T>();
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _dbSet.Where(e => e.DeletedAt == null).ToListAsync();
        }

        public async Task<T> GetByIdAsync(int id)
        {
            return await _dbSet.FirstOrDefaultAsync(e => e.Id == id && e.DeletedAt == null);
        }
        public async Task<T> AddAsync(T entity)
        {
            await _dbSet.AddAsync(entity);
            return entity;
        }

        public Task Update(T entity)
        {
            entity.UpdatedAt = DateTime.UtcNow;
           _dbSet.Update(entity);
            return Task.CompletedTask;
        }

        public Task Delete(T entity)
        {
            entity.DeletedAt = DateTime.UtcNow;
            _dbSet.Update(entity);
            return Task.CompletedTask;
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _dbSet.AnyAsync(e => e.Id == id && e.DeletedAt == null);
        }

      

       
    }
}
