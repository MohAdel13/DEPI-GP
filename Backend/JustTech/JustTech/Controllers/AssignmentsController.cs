using JustTech.Business.Services;
using JustTech.Core.DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace JustTech.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AssignmentsController : ControllerBase
    {
        private readonly IAssignmentService _assignmentService;

        public AssignmentsController(IAssignmentService assignmentService)
        {
            _assignmentService = assignmentService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var assignments = await _assignmentService.GetAllAsync();
            return Ok(assignments);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var assignment = await _assignmentService.GetByIdAsync(id);
            if (assignment == null)
                return NotFound(new { message = $"Assignment with ID {id} not found" });

            return Ok(assignment);
        }

        [HttpGet("round/{roundId}")]
        public async Task<IActionResult> GetByRoundId(int roundId)
        {
            var assignments = await _assignmentService.GetAssignmentsByRoundIdAsync(roundId);
            if (!assignments.Any())
                return NotFound(new { message = $"No assignments found for Round with ID {roundId}" });

            return Ok(assignments);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateAssignmentDto createDto)
        {
            var assignment = await _assignmentService.CreateAsync(createDto);
            return CreatedAtAction(nameof(GetById), new { id = assignment.Id }, assignment);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateAssignmentDto updateDto)
        {
            var assignment = await _assignmentService.UpdateAsync(id, updateDto);
            if (assignment == null)
                return NotFound(new { message = $"Assignment with ID {id} not found" });

            return Ok(assignment);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var deleted = await _assignmentService.DeleteAsync(id);
            if (!deleted)
                return NotFound(new { message = $"Assignment with ID {id} not found" });

            return NoContent();
        }

    }
}
