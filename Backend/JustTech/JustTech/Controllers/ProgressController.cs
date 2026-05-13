using JustTech.Business.Services;
using JustTech.Core.DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace JustTech.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProgressController : ControllerBase
    {
        private readonly IProgressService _progressService;

        public ProgressController(IProgressService progressService)
        {
            _progressService = progressService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var progresses = await _progressService.GetAllAsync();
            return Ok(progresses);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var progress = await _progressService.GetByIdAsync(id);
            if (progress == null)
                return NotFound(new { message = $"Progress with ID {id} not found" });

            return Ok(progress);
        }

        [HttpGet("student/{studentId}/lecture/{lectureId}")]
        public async Task<IActionResult> GetByStudentAndLecture(int studentId, int lectureId)
        {
            var progress = await _progressService.GetProgressByStudentAndLectureAsync(studentId, lectureId);
            if (progress == null)
                return NotFound(new { message = $"Progress not found for Student {studentId} and Lecture {lectureId}" });

            return Ok(progress);
        }

        [HttpGet("student/{studentId}")]
        public async Task<IActionResult> GetByStudentId(int studentId)
        {
            var progresses = await _progressService.GetProgressByStudentIdAsync(studentId);
            if (!progresses.Any())
                return NotFound(new { message = $"No progress records found for Student with ID {studentId}" });

            return Ok(progresses);
        }

        [HttpGet("lecture/{lectureId}")]
        public async Task<IActionResult> GetByLectureId(int lectureId)
        {
            var progresses = await _progressService.GetProgressByLectureIdAsync(lectureId);
            if (!progresses.Any())
                return NotFound(new { message = $"No progress records found for Lecture with ID {lectureId}" });

            return Ok(progresses);
        }

        [HttpPost]
        public async Task<IActionResult> CreateOrUpdate([FromBody] CreateProgressDto createDto)
        {
            var progress = await _progressService.CreateOrUpdateProgressAsync(createDto);
            if (progress == null)
                return BadRequest(new { message = "Student or Lecture not found" });

            return Ok(progress);
        }


        [HttpPost("complete")]
        public async Task<IActionResult> MarkLectureCompleted([FromBody] WatchLectureDto watchDto)
        {
            var progress = await _progressService.MarkLectureCompletedAsync(watchDto.StudentId, watchDto.LectureId);
            if (progress == null)
                return BadRequest(new { message = "Student or Lecture not found" });

            return Ok(progress);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateProgressDto updateDto)
        {
            var progress = await _progressService.UpdateProgressAsync(id, updateDto);
            if (progress == null)
                return NotFound(new { message = $"Progress with ID {id} not found" });

            return Ok(progress);
        }

        [HttpGet("student/{studentId}/completed-count")]
        public async Task<IActionResult> GetCompletedCount(int studentId)
        {
            var count = await _progressService.GetCompletedLecturesCountAsync(studentId);
            return Ok(new { studentId, completedLecturesCount = count });
        }

        [HttpGet("student/{studentId}/round/{roundId}/percentage")]
        public async Task<IActionResult> GetProgressPercentage(int studentId, int roundId)
        {
            var percentage = await _progressService.GetStudentProgressPercentageAsync(studentId, roundId);
            return Ok(new { studentId, roundId, progressPercentage = percentage });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var deleted = await _progressService.DeleteAsync(id);
            if (!deleted)
                return NotFound(new { message = $"Progress with ID {id} not found" });

            return NoContent();
        }

    }
}
