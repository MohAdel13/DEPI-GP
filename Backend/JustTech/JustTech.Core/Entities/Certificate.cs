using System;
using System.Collections.Generic;
using System.Text;

namespace JustTech.Core.Entities
{
    public class Certificate : BaseEntity
    {
        public int StudentId { get; set; }
        public int RoundId { get; set; }
        public DateTime? IssuedAt { get; set; }
        public string? Url { get; set; }


        // Navigation Properties
        public virtual Student Student { get; set; }
        public virtual Round Round { get; set; }
    }
}
