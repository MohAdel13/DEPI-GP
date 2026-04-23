using System;
using System.Collections.Generic;
using System.Text;

namespace JustTech.Core.Entities
{
    public class Course : BaseEntity
    {
        public string Name { get; set; }
        public string? Description { get; set; }
        public string? CoursePlan { get; set; }


        // Navigation Property
        public virtual ICollection<Round> Rounds { get; set; }
    }
}
