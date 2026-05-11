namespace JustTech.Core.DTOs
{
    public class CreateSubmissionDto
    {
        public int AssignmentId { get; set; }
        public int StudentId { get; set; }
        public string? AnswerUrl { get; set; }
    }
}
