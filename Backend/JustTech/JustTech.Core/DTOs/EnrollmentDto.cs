namespace JustTech.Core.DTOs
{
    public class EnrollmentDto
    {
        public int Id { get; set; }
        public int StudentId { get; set; }
        public string StudentName { get; set; }
        public int RoundId { get; set; }
        public string RoundName { get; set; }
        public int CourseId { get; set; }
        public string CourseName { get; set; }
        public string Status { get; set; }
        public DateTime EnrolledAt { get; set; }
        public DateTime CreatedAt { get; set; }
    }

    public class CreateEnrollmentDto
    {
        public int StudentId { get; set; }
        public int RoundId { get; set; }
    }

    public class UpdateEnrollmentStatusDto
    {
        public string Status { get; set; } // active / inactive
    }
}
