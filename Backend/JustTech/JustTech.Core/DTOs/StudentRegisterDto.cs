using System.ComponentModel.DataAnnotations;

namespace JustTech.Core.DTOs
{
    public class StudentRegisterDto
    {
        [Required]
        [MaxLength(255)]
        public string Name { get; set; }

        [Required]
        [EmailAddress]
        [MaxLength(255)]
        public string Email { get; set; }

        [Required]
        [MinLength (6)]
        [MaxLength (255)]
        public string Password { get; set; }

        [Phone]
        [MaxLength(20)]
        public string? Phone { get; set; }

        [MaxLength(100)]
        public string? Profession { get; set; }

        [MaxLength(100)]
        public string? City { get; set; }

        [MaxLength(100)]
        public string? Country { get; set; }

        [MaxLength(20)]
        public string? StudentStatus { get; set; }

        public DateTime? Birthdate { get; set; }
    }
}
