using JustTech.Business.Services;
using JustTech.Core.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace JustTech.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoundsController : ControllerBase
    {

        private readonly IRoundService _roundService;

        public RoundsController(IRoundService roundService)
        {
            _roundService = roundService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var rounds = await _roundService.GetAllAsync();

            return Ok(rounds);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var round = await _roundService.GetByIdAsync(id);
            if (round == null)
                return NotFound(new { message = $"Round with ID {id} not found" });

            return Ok(round);
        }

        [HttpGet("course/{courseId}")]
        public async Task<IActionResult> GetByCourseId(int courseId)
        {
            var rounds = await _roundService.GetRoundsByCourseIdAsync(courseId);
            if (!rounds.Any())
                return NotFound(new { message = $"No rounds found for Course with ID {courseId}" });

            return Ok(rounds);
        }


        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateRoundDto createDto)
        {
            var round = await _roundService.CreateAsync(createDto);
            if (round == null)
                return BadRequest(new { message = "Course or Instructor not found" });

            return CreatedAtAction(nameof(GetById), new { id = round.Id }, round);
        }


        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateRoundDto updateDto)
        {
            var round = await _roundService.UpdateAsync(id, updateDto);
            if (round == null)
                return NotFound(new { message = $"Round with ID {id} not found" });

            return Ok(round);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var deleted = await _roundService.DeleteAsync(id);
            if (!deleted)
                return NotFound(new { message = $"Round with ID {id} not found" });

            return NoContent();
        }
    }
}
