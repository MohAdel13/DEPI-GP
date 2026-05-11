using JustTech.Core.Interfaces;
using JustTech.Infrastructure.Data;

namespace JustTech.Infrastructure.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {

        private readonly AppDbContext _context;
        private ICourseRepository _courseRepository;
        private IStudentRepository _studentRepository;
        private IRoundRepository _roundRepository;
        private IEnrollmentRepository _enrollmentRepository;
        private IInstructorRepository _instructorRepository;
        public UnitOfWork(AppDbContext context)
        {
            _context = context;
        }
        public ICourseRepository Courses => _courseRepository ??= new CourseRepository(_context);
        public IStudentRepository Students => _studentRepository ??= new StudentRepository(_context);

        public IRoundRepository Rounds => _roundRepository ??= new RoundRepository(_context);
        public IEnrollmentRepository Enrollments => _enrollmentRepository ??= new EnrollmentRepository(_context);
        public IInstructorRepository Instructors => _instructorRepository ??= new InstructorRepository(_context);

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
