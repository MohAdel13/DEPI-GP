namespace JustTech.Core.DTOs
{
    public class MaterialDto
    {
        public int Id { get; set; }
        public int LectureId { get; set; }
        public string LectureTitle { get; set; }
        public string Url { get; set; }
        public string Type { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
