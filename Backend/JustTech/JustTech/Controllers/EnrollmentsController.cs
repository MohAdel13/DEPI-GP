using JustTech.Business.Services;
using JustTech.Core.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace JustTech.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EnrollmentsController : ControllerBase
    {
        private readonly IEnrollmentService _enrollmentService;

        public EnrollmentsController(IEnrollmentService enrollmentService)
        {
            _enrollmentService = enrollmentService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var enrollments = await _enrollmentService.GetAllAsync();
            return Ok(enrollments);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var enrollment = await _enrollmentService.GetByIdAsync(id);

            return Ok(enrollment);
        }

        [HttpGet("student/{studentId}")]
        public async Task<IActionResult> GetByStudentId(int studentId)
        {
            var enrollments = await _enrollmentService.GetEnrollmentsByStudentIdAsync(studentId);
            return Ok(enrollments);
        }

        [HttpGet("round/{roundId}")]
        public async Task<IActionResult> GetByRoundId(int roundId)
        {
            var enrollments = await _enrollmentService.GetEnrollmentsByRoundIdAsync(roundId);
            return Ok(enrollments);
        }

        [HttpPost]
        public async Task<IActionResult> Enroll([FromBody] CreateEnrollmentDto createDto)
        {
            var enrollment = await _enrollmentService.EnrollStudentAsync(createDto);
            return CreatedAtAction(nameof(GetById), new { id = enrollment.Id }, enrollment);
        }

        [HttpPut("{id}/status")]
        public async Task<IActionResult> UpdateStatus(int id, [FromBody] UpdateEnrollmentStatusDto updateDto)
        {
            var enrollment = await _enrollmentService.UpdateEnrollmentStatusAsync(id, updateDto);
            return Ok(enrollment);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var enrollment = await _enrollmentService.GetByIdAsync(id);
            if (enrollment == null)
                return NotFound($"Enrollment with ID {id} not found");

            await _enrollmentService.DeleteAsync(id);
            return NoContent();
        }

    }
}
