using System;
using System.Collections.Generic;
using System.Text;

namespace JustTech.Core.Entities
{
    public class Enrollment : BaseEntity
    {
        public int StudentId { get; set; }
        public int RoundId { get; set; }
        public string? Status { get; set; } // active / inactive
        public DateTime? EnrolledAt { get; set; }

        // Navigation  Properties
        public virtual Student Student { get; set; }
        public virtual Round Round { get; set; }
    }
}
