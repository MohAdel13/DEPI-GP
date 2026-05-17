using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using JustTech.Data;
using JustTech.DTOs;
using JustTech.Models;

namespace JustTech.Controllers
{
    [ApiController]
    [Route("api/instructor/courses/{courseId}/sections")]
    [Authorize(Roles = "Instructor")]
    public class InstructorSectionsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public InstructorSectionsController(AppDbContext context)
        {
            _context = context;
        }

        // ================= CREATE SECTION =================
        [HttpPost]
        public async Task<IActionResult> AddSection(int courseId, CreateSectionDto dto)
        {
            var section = new Section
            {
                Title = dto.Title,
                CourseId = courseId
            };

            _context.Sections.Add(section);
            await _context.SaveChangesAsync();

            return Ok(section);
        }

        // ================= GET ALL SECTIONS =================
        [HttpGet]
        public async Task<IActionResult> GetSections(int courseId)
        {
            var sections = await _context.Sections
                .Where(s => s.CourseId == courseId)
                .ToListAsync();

            return Ok(sections);
        }

        // ================= GET SINGLE SECTION =================
        [HttpGet("{sectionId}")]
        public async Task<IActionResult> GetSection(int courseId, int sectionId)
        {
            var section = await _context.Sections
                .FirstOrDefaultAsync(s => s.Id == sectionId && s.CourseId == courseId);

            if (section == null)
                return NotFound();

            return Ok(section);
        }

        // ================= UPDATE SECTION =================
        [HttpPut("{sectionId}")]
        public async Task<IActionResult> UpdateSection(int courseId, int sectionId, CreateSectionDto dto)
        {
            var section = await _context.Sections
                .FirstOrDefaultAsync(s => s.Id == sectionId && s.CourseId == courseId);

            if (section == null)
                return NotFound();

            section.Title = dto.Title;

            await _context.SaveChangesAsync();

            return Ok(section);
        }

        // ================= DELETE SECTION =================
        [HttpDelete("{sectionId}")]
        public async Task<IActionResult> DeleteSection(int courseId, int sectionId)
        {
            var section = await _context.Sections
                .FirstOrDefaultAsync(s => s.Id == sectionId && s.CourseId == courseId);

            if (section == null)
                return NotFound();

            _context.Sections.Remove(section);
            await _context.SaveChangesAsync();

            return Ok("Section deleted");
        }
    }
}