using JustTech.Core.Interfaces;
using JustTech.Infrastructure.Data;

namespace JustTech.Infrastructure.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {

        private readonly AppDbContext _context;
        private ICourseRepository _courseRepository;
        public UnitOfWork(AppDbContext context)
        {
            _context = context;
        }
        public ICourseRepository Courses => _courseRepository ??= new CourseRepository(_context);

        /*
         What => _courseRepository ??= new CourseRepository(_context) does:
                This is a property that lazy-initializes CourseRepository.

        Example: 

        public ICourseRepository Courses 
{
    get 
    {
        if (_courseRepository == null)
        {
            _courseRepository = new CourseRepository(_context);
        }
        return _courseRepository;
    }
}


        ??= means: If _courseRepository is null, create new instance, then assign it.
         */
        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
