using JustTech.Business.Services;
using JustTech.Core.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace JustTech.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InstructorsController : ControllerBase
    {
        private readonly IInstructorService _instructorService;

        public InstructorsController(IInstructorService instructorService)
        {
            _instructorService = instructorService;
        }


        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var instructors = await _instructorService.GetAllAsync();
            return Ok(instructors);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var instructor = await _instructorService.GetByIdAsync(id);
            if (instructor == null)
                return NotFound(new { message = $"Instructor with ID {id} not found" });

            return Ok(instructor);
        }

        [HttpGet("email/{email}")]
        public async Task<IActionResult> GetByEmail(string email)
        {
            var instructor = await _instructorService.GetByEmailAsync(email);
            if (instructor == null)
                return NotFound(new { message = $"Instructor with email '{email}' not found" });

            return Ok(instructor);
        }

        [HttpGet("profession/{profession}")]
        public async Task<IActionResult> GetByProfession(string profession)
        {
            var instructors = await _instructorService.GetInstructorsByProfessionAsync(profession);
            if (!instructors.Any())
                return NotFound(new { message = $"No instructors found with profession '{profession}'" });

            return Ok(instructors);
        }

        [HttpGet("city/{city}")]
        public async Task<IActionResult> GetByCity(string city)
        {
            var instructors = await _instructorService.GetInstructorsByCityAsync(city);
            if (!instructors.Any())
                return NotFound(new { message = $"No instructors found in city '{city}'" });

            return Ok(instructors);
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Create([FromBody] CreateInstructorDto createDto)
        {
            var instructor = await _instructorService.CreateAsync(createDto);
            return CreatedAtAction(nameof(GetById), new { id = instructor.Id }, instructor);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateInstructorDto updateDto)
        {
            var instructor = await _instructorService.UpdateAsync(id, updateDto);
            if (instructor == null)
                return NotFound(new { message = $"Instructor with ID {id} not found" });

            return Ok(instructor);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var deleted = await _instructorService.DeleteAsync(id);
            if (!deleted)
                return NotFound(new { message = $"Instructor with ID {id} not found" });

            return NoContent();
        }
    }
}
