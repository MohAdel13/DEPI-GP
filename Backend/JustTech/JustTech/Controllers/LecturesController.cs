using JustTech.Business.Services;
using JustTech.Core.DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace JustTech.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LecturesController : ControllerBase
    {
        private readonly ILectureService _lectureService;

        public LecturesController(ILectureService lectureService)
        {
            _lectureService = lectureService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var lectures = await _lectureService.GetAllAsync();
            return Ok(lectures);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var lecture = await _lectureService.GetByIdAsync(id);
            return Ok(lecture);
        }

        [HttpGet("round/{roundId}")]
        public async Task<IActionResult> GetByRoundId(int roundId)
        {
            var lectures = await _lectureService.GetLecturesByRoundIdAsync(roundId);
            return Ok(lectures);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateLectureDto createDto)
        {
            var lecture = await _lectureService.CreateAsync(createDto);
            return CreatedAtAction(nameof(GetById), new { id = lecture.Id }, lecture);
        }   

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateLectureDto updateDto)
        {
            var lecture = await _lectureService.UpdateAsync(id, updateDto);
            return Ok(lecture);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var lecture = await _lectureService.GetByIdAsync(id);
            if (lecture == null)
                return NotFound($"Lecture with ID {id} not found");

            await _lectureService.DeleteAsync(id);
            return NoContent();
        }
    }
}
