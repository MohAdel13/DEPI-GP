using static System.Collections.Specialized.BitVector32;

namespace JustTech.Models
{
    public class Course
    {
        public int Id { get; set; }

        public string Title { get; set; }  

        public string Description { get; set; }
        public int CoursePrice { get; set; }

        public int InstructorId { get; set; }

        public AppUser Instructor { get; set; }

        //public List<Section> Sections { get; set; } = new();
    }
}
