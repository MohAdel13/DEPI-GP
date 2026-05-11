namespace JustTech.Core.DTOs
{
    public class CreateAssignmentDto
    {
        public int RoundId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime? DueDate { get; set; }
    }
}
