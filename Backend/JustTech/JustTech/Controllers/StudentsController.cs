using JustTech.Business.Services;
using JustTech.Core.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace JustTech.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentsController : ControllerBase
    {
        private readonly IStudentService _studentService;

        public StudentsController(IStudentService studentService)
        {
            _studentService = studentService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllAsync()
        {
            var students = await _studentService.GetAllAsync();
            return Ok(students);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var student = await _studentService.GetByIdAsync(id);
            if (student == null)
                return NotFound(new { message = $"Student with ID {id} not found" });

            return Ok(student);
        }

        [HttpGet("status/{status}")]
        public async Task<IActionResult> GetByStatus(string status)
        {
            var students = await _studentService.GetStudentsByStatusAsync(status);
            if (!students.Any())
                return NotFound(new { message = $"No students found with status '{status}'" });

            return Ok(students);
        }

        [HttpGet("city/{city}")]
        public async Task<IActionResult> GetByCity(string city)
        {
            var students = await _studentService.GetStudentsByCityAsync(city);
            if (!students.Any())
                return NotFound(new { message = $"No students found in city '{city}'" });

            return Ok(students);
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Create([FromBody] CreateStudentDto createDto)
        {
            var student = await _studentService.CreateAsync(createDto);
            return CreatedAtAction(nameof(GetById), new { id = student.Id }, student);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateStudentDto updateDto)
        {
            var student = await _studentService.UpdateAsync(id, updateDto);
            if (student == null)
                return NotFound(new { message = $"Student with ID {id} not found" });

            return Ok(student);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var deleted = await _studentService.DeleteAsync(id);
            if (!deleted)
                return NotFound(new { message = $"Student with ID {id} not found" });

            return NoContent();
        }
    }
}
