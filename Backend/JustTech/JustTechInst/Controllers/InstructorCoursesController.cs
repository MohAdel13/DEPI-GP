using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using JustTech.Data;
using JustTech.DTOs;
using JustTech.Models;
using System.Security.Claims;

namespace JustTech.Controllers
{
    [ApiController]
    [Route("api/instructor/courses")]
    [Authorize(Roles = "Instructor")]
    public class InstructorCoursesController : ControllerBase
    {
        private readonly AppDbContext _context;

        public InstructorCoursesController(AppDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<IActionResult> CreateCourse(CreateCourseDto dto)
        {
            var instructorId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

            var course = new Course
            {
                Title = dto.Title,
                Description = dto.Description,
                CoursePrice = 100
                , 
                InstructorId = instructorId
            };

            _context.Courses.Add(course);
            await _context.SaveChangesAsync();

            return Ok(course);
        }

        [HttpGet]
        public async Task<IActionResult> GetMyCourses()
        {
            var instructorId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

            var courses = await _context.Courses
                .Where(c => c.InstructorId == instructorId)
                .ToListAsync();

            return Ok(courses);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetCourse(int id)
        {
            var instructorId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

            var course = await _context.Courses
                .FirstOrDefaultAsync(c => c.Id == id && c.InstructorId == instructorId);

            if (course == null)
                return NotFound();

            return Ok(course);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCourse(int id, UpdateCourseDto dto)
        {
            var instructorId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

            var course = await _context.Courses
                .FirstOrDefaultAsync(c => c.Id == id && c.InstructorId == instructorId);

            if (course == null)
                return NotFound();

            course.Title = dto.Title;
            course.Description = dto.Description;

            await _context.SaveChangesAsync();

            return Ok(course);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCourse(int id)
        {
            var instructorId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

            var course = await _context.Courses
                .FirstOrDefaultAsync(c => c.Id == id && c.InstructorId == instructorId);

            if (course == null)
                return NotFound();

            _context.Courses.Remove(course);
            await _context.SaveChangesAsync();

            return Ok("Course Deleted");
        }
    }
}