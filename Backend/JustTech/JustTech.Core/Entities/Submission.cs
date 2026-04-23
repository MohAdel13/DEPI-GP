using System;
using System.Collections.Generic;
using System.Text;

namespace JustTech.Core.Entities
{
    public class Submission : BaseEntity
    {
        public int AssignmentId { get; set; }
        public int StudentId { get; set; }
        
        public string? AnswerUrl { get; set; }
        public decimal? Grade { get; set; }
        public DateTime? SubmittedAt { get; set; }
        public string? Status { get; set; } // submitted / not submitted / late

        // Navigation Properties
        public virtual Assignment Assignment { get; set; }
        public virtual Student Student { get; set; }
    }
}
