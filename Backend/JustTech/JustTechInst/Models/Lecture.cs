namespace JustTech.Models
{
    public class Lecture
    {
        public int Id { get; set; }

        public string Title { get; set; } = string.Empty;

        public string VideoUrl { get; set; } = string.Empty;

        public int SectionId { get; set; }

        public Section Section { get; set; } = null!;
    }
}