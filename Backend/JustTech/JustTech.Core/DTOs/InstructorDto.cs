namespace JustTech.Core.DTOs
{
    public class InstructorDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string? Phone { get; set; }
        public string? Profession { get; set; }
        public string? City { get; set; }
        public string? Country { get; set; }
        public DateTime? Birthdate { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
