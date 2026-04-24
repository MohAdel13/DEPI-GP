using System;
using System.Collections.Generic;
using System.Text;
using JustTech.Core.Entities;
using JustTech.Core.Interfaces;
using JustTech.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;


namespace JustTech.Infrastructure.Repositories
{
    public class CourseRepository : Repository<Course>, ICourseRepository
    {
        private readonly AppDbContext _context;

        public CourseRepository(AppDbContext context) : base (context)
        {   
            _context = context;
        }
        public async Task<IEnumerable<Course>> GetAllCoursesAsync()
        {
            return await _context.Courses.Where(c => c.DeletedAt == null).ToListAsync();
        }

        public async Task<Course?> GetCourseWithRoundsAsync(int courseId)
        {
            return await _context.Courses
                .Include(c => c.Rounds)
                .FirstOrDefaultAsync(c => c.Id == courseId && c.DeletedAt == null);
        }
    }
}
