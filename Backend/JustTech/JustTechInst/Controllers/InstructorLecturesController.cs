using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using JustTech.Data;
using JustTech.DTOs;
using JustTech.Models;

namespace JustTech.Controllers
{
    [ApiController]
    [Route("api/instructor/lectures")]
    [Authorize(Roles = "Instructor")]
    public class InstructorLecturesController : ControllerBase
    {
        private readonly AppDbContext _context;

        public InstructorLecturesController(AppDbContext context)
        {
            _context = context;
        }

        //  CREATE LECTURE 
        [HttpPost("{sectionId}")]
        public async Task<IActionResult> AddLecture(int sectionId, CreateLectureDto dto)
        {
            var lecture = new Lecture
            {
                Title = dto.Title,
                VideoUrl = dto.VideoUrl, 
                SectionId = sectionId
            };

            _context.Lectures.Add(lecture);
            await _context.SaveChangesAsync();

            return Ok(lecture);
        }

        // UPDATE VIDEO URL 
        [HttpPut("{lectureId}/video")]
        public async Task<IActionResult> UpdateVideo(int lectureId, [FromBody] string videoUrl)
        {
            var lecture = await _context.Lectures
                .FirstOrDefaultAsync(l => l.Id == lectureId);

            if (lecture == null)
                return NotFound();

            lecture.VideoUrl = videoUrl;

            await _context.SaveChangesAsync();

            return Ok(new
            {
                message = "Video updated successfully",
                lectureId,
                videoUrl
            });
        }

        //  GET LECTURE BY ID 
        [HttpGet("{lectureId}")]
        public async Task<IActionResult> GetLecture(int lectureId)
        {
            var lecture = await _context.Lectures
                .FirstOrDefaultAsync(l => l.Id == lectureId);

            if (lecture == null)
                return NotFound();

            return Ok(lecture);
        }

        //  DELETE LECTURE 
        [HttpDelete("{lectureId}")]
        public async Task<IActionResult> DeleteLecture(int lectureId)
        {
            var lecture = await _context.Lectures
                .FirstOrDefaultAsync(l => l.Id == lectureId);

            if (lecture == null)
                return NotFound();

            _context.Lectures.Remove(lecture);
            await _context.SaveChangesAsync();

            return Ok("Lecture deleted");
        }
    }
}