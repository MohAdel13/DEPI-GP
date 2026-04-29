using System.ComponentModel.DataAnnotations;

namespace JustTech.Core.DTOs
{
    public class StudentLoginDto
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
