namespace JustTech.Models
{
    public class Section
    {
        public int Id { get; set; }

        public string Title { get; set; } = string.Empty;

        public int CourseId { get; set; }

        public Course Course { get; set; } = null!;
    }
}