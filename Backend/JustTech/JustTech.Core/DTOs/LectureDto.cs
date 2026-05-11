namespace JustTech.Core.DTOs
{
    public class LectureDto
    {
        public int Id { get; set; }
        public int RoundId { get; set; }
        public string RoundName { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int MaterialsCount { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
