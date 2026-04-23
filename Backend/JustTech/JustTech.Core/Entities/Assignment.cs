using System;
using System.Collections.Generic;
using System.Text;

namespace JustTech.Core.Entities
{
    public class Assignment : BaseEntity
    {
        public int RoundId { get; set; }
        public string Title { get; set; }
        public string? Description { get; set; }
        public DateTime? DueDate { get; set; }

        // Navigation Properties
        public virtual Round Round { get; set; }
        public virtual ICollection<Submission> Submissions { get; set; }
    }
}
