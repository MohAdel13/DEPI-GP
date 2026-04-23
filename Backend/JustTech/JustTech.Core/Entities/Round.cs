using System;
using System.Collections.Generic;
using System.Text;

namespace JustTech.Core.Entities
{
    public class Round : BaseEntity
    {
        public string Name { get; set; }
        public int CourseId { get; set; }
        public int InstructorId { get; set; }
        public DateTime? StartsAt { get; set; }
        public DateTime? EndsAt { get; set; }
        public string? Status { get; set; } // in progress / completed / cancelled / delayed



        // Naviation Property
        public virtual Course Course { get; set; }
        public virtual Instructor Instructor { get; set; }
        public virtual ICollection<Enrollment> Enrollments { get; set; }
        public virtual ICollection<Lecture> Lectures { get; set; }
        public virtual ICollection<Assignment> Assignments { get; set; }
        public virtual ICollection<Certificate> Certificates { get; set; }

    }
}
