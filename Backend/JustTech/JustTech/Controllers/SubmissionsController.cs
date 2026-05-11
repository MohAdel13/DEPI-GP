using JustTech.Business.Services;
using JustTech.Core.DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace JustTech.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SubmissionsController : ControllerBase
    {
        private readonly ISubmissionService _submissionService;

        public SubmissionsController(ISubmissionService submissionService)
        {
            _submissionService = submissionService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var submissions = await _submissionService.GetAllAsync();
            return Ok(submissions);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var submission = await _submissionService.GetByIdAsync(id);
            if (submission == null)
                return NotFound(new { message = $"Submission with ID {id} not found" });

            return Ok(submission);
        }

        [HttpGet("assignment/{assignmentId}")]
        public async Task<IActionResult> GetByAssignmentId(int assignmentId)
        {
            var submissions = await _submissionService.GetSubmissionsByAssignmentIdAsync(assignmentId);
            if (!submissions.Any())
                return NotFound(new { message = $"No submissions found for Assignment with ID {assignmentId}" });

            return Ok(submissions);
        }

        [HttpGet("student/{studentId}")]
        public async Task<IActionResult> GetByStudentId(int studentId)
        {
            var submissions = await _submissionService.GetSubmissionsByStudentIdAsync(studentId);
            if (!submissions.Any())
                return NotFound(new { message = $"No submissions found for Student with ID {studentId}" });

            return Ok(submissions);
        }

        [HttpGet("assignment/{assignmentId}/student/{studentId}")]
        public async Task<IActionResult> GetByAssignmentAndStudent(int assignmentId, int studentId)
        {
            var submission = await _submissionService.GetSubmissionByAssignmentAndStudentAsync(assignmentId, studentId);
            if (submission == null)
                return NotFound(new { message = $"No submission found for Assignment {assignmentId} and Student {studentId}" });

            return Ok(submission);
        }

        [HttpPost]
        public async Task<IActionResult> CreateOrUpdate([FromBody] CreateSubmissionDto createDto)
        {
            var submission = await _submissionService.CreateOrUpdateSubmissionAsync(createDto);
            if (submission == null)
                return BadRequest(new { message = "Invalid Assignment ID or Student ID" });

            return Ok(submission);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateSubmissionDto updateDto)
        {
            var submission = await _submissionService.UpdateSubmissionAsync(id, updateDto);
            if (submission == null)
                return NotFound(new { message = $"Submission with ID {id} not found" });

            return Ok(submission);
        }

        [HttpPatch("{id}/grade")]
        public async Task<IActionResult> Grade(int id, [FromBody] GradeSubmissionDto gradeDto)
        {
            if (gradeDto.Grade < 0 || gradeDto.Grade > 100)
                return BadRequest(new { message = "Grade must be between 0 and 100" });

            var submission = await _submissionService.GradeSubmissionAsync(id, gradeDto);
            if (submission == null)
                return NotFound(new { message = $"Submission with ID {id} not found" });

            return Ok(submission);
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var deleted = await _submissionService.DeleteAsync(id);
            if (!deleted)
                return NotFound(new { message = $"Submission with ID {id} not found" });

            return NoContent();
        }



    }
}
