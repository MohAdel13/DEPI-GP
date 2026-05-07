namespace JustTech.Core.DTOs
{
    public class RoundDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int CourseId { get; set; }
        public string CourseName { get; set; }
        public int InstructorId { get; set; }
        public string InstructorName { get; set; }
        public DateTime? StartsAt { get; set; }
        public DateTime? EndsAt { get; set; }
        public string? Status { get; set; }
        public int EnrollmentsCount { get; set; }
        public DateTime CreatedAt { get; set; }
    }


    public class CreateRoundDto
    {
        public string Name { get; set; }
        public int CourseId { get; set; }
        public int InstructorId { get; set; }
        public DateTime? StartsAt { get; set; }
        public DateTime? EndsAt { get; set; }
        public string? Status { get; set; }
    }

    public class UpdateRoundDto
    {
        public string Name { get; set; }
        public DateTime? StartsAt { get; set; }
        public DateTime? EndsAt { get; set; }
        public string? Status { get; set; }
    }
}
