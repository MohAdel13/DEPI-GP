using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using JustTech.Data;
using JustTech.DTOs;
using System.Security.Claims;

namespace JustTech.Controllers
{
    [ApiController]
    [Route("api/instructor/profile")]
    [Authorize(Roles = "Instructor")]
    public class InstructorProfileController : ControllerBase
    {
        private readonly AppDbContext _context;

        public InstructorProfileController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetProfile()
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

            var instructor = await _context.Users.FindAsync(userId);

            if (instructor == null)
                return NotFound();

            return Ok(instructor);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateProfile(UpdateProfileDto dto)
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

            var instructor = await _context.Users.FindAsync(userId);

            if (instructor == null)
                return NotFound();

            instructor.Name = dto.Name;
            instructor.Email = dto.Email;

            await _context.SaveChangesAsync();

            return Ok(instructor);
        }
    }
}