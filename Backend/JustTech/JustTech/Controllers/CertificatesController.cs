using JustTech.Business.Services;
using JustTech.Core.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace JustTech.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CertificatesController : ControllerBase
    {
        private readonly ICertificateService _certificateService;

        public CertificatesController(ICertificateService certificateService)
        {
            _certificateService = certificateService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var certificates = await _certificateService.GetAllAsync();
            return Ok(certificates);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var certificate = await _certificateService.GetByIdAsync(id);
            if (certificate == null)
                return NotFound(new { message = $"Certificate with ID {id} not found" });

            return Ok(certificate);
        }

        [HttpGet("student/{studentId}")]
        public async Task<IActionResult> GetByStudentId(int studentId)
        {
            var certificates = await _certificateService.GetCertificatesByStudentIdAsync(studentId);
            if (!certificates.Any())
                return NotFound(new { message = $"No certificates found for Student with ID {studentId}" });

            return Ok(certificates);
        }

        [HttpGet("round/{roundId}")]
        public async Task<IActionResult> GetByRoundId(int roundId)
        {
            var certificates = await _certificateService.GetCertificatesByRoundIdAsync(roundId);
            if (!certificates.Any())
                return NotFound(new { message = $"No certificates found for Round with ID {roundId}" });

            return Ok(certificates);
        }

        [HttpGet("student/{studentId}/round/{roundId}")]
        public async Task<IActionResult> GetByStudentAndRound(int studentId, int roundId)
        {
            var certificate = await _certificateService.GetCertificateByStudentAndRoundAsync(studentId, roundId);
            if (certificate == null)
                return NotFound(new { message = $"No certificate found for Student {studentId} and Round {roundId}" });

            return Ok(certificate);
        }

        [HttpPost("generate")]
        public async Task<IActionResult> Generate([FromBody] GenerateCertificateDto generateDto)
        {
            var certificate = await _certificateService.GenerateCertificateAsync(generateDto);
            if (certificate == null)
                return BadRequest(new { message = "Cannot generate certificate. Student, Round not found, or progress not completed (100%)" });

            return CreatedAtAction(nameof(GetById), new { id = certificate.Id }, certificate);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateCertificateDto updateDto)
        {
            var certificate = await _certificateService.UpdateCertificateAsync(id, updateDto);
            if (certificate == null)
                return NotFound(new { message = $"Certificate with ID {id} not found" });

            return Ok(certificate);
        }
        [HttpGet("student/{studentId}/round/{roundId}/exists")]
        public async Task<IActionResult> HasCertificate(int studentId, int roundId)
        {
            var exists = await _certificateService.HasCertificateAsync(studentId, roundId);
            return Ok(new { studentId, roundId, hasCertificate = exists });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var deleted = await _certificateService.DeleteAsync(id);
            if (!deleted)
                return NotFound(new { message = $"Certificate with ID {id} not found" });

            return NoContent();
        }



    }
}
