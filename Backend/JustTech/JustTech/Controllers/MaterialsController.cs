using JustTech.Business.Services;
using JustTech.Core.DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace JustTech.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MaterialsController : ControllerBase
    {
        private readonly IMaterialService _materialService;

        public MaterialsController(IMaterialService materialService)
        {
            _materialService = materialService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var materials = await _materialService.GetAllAsync();
            return Ok(materials);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var material = await _materialService.GetByIdAsync(id);
            if (material == null)
                return NotFound(new { message = $"Material with ID {id} not found" });

            return Ok(material);
        }

        [HttpGet("lecture/{lectureId}")]
        public async Task<IActionResult> GetByLectureId(int lectureId)
        {
            var materials = await _materialService.GetMaterialsByLectureIdAsync(lectureId);
            if (!materials.Any())
                return NotFound(new { message = $"No materials found for Lecture with ID {lectureId}" });

            return Ok(materials);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateMaterialDto createDto)
        {
            var material = await _materialService.CreateAsync(createDto);
            if (material == null)
                return BadRequest(new { message = "Lecture not found" });

            return CreatedAtAction(nameof(GetById), new { id = material.Id }, material);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateMaterialDto updateDto)
        {
            var material = await _materialService.UpdateAsync(id, updateDto);
            if (material == null)
                return NotFound(new { message = $"Material with ID {id} not found" });

            return Ok(material);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var deleted = await _materialService.DeleteAsync(id);
            if (!deleted)
                return NotFound(new { message = $"Material with ID {id} not found" });

            return NoContent();
        }
    }
}
