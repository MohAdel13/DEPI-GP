using Business.Logic.Services;
using JustTech.Core.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace JustTech.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CoursesController : ControllerBase
    {
        private ICourseService _courseService;

        public CoursesController(ICourseService courseService)
        {
            _courseService = courseService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var courses = await _courseService.GetAllAsync();
            return Ok(courses);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var course = await _courseService.GetByIdAsync(id);
            return Ok(course);
        }


        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateCourseDto createCourseDto)
        {
            var course = await _courseService.CreateAsync(createCourseDto);
            return CreatedAtAction(nameof(GetById), new { id = course.Id }, course);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateCourseDto updateCourseDto)
        {
            var course = await _courseService.UpdateAsync(id, updateCourseDto);
            return Ok(course);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var course = await _courseService.GetByIdAsync(id);
            if (course == null)
                return NotFound($"Course with Id {id} not found");

            await _courseService.DeleteAsync(id);
            return NoContent();
        }
    }
}
