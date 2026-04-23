using System;
using System.Collections.Generic;
using System.Text;

namespace JustTech.Core.Entities
{
    public class Lecture : BaseEntity
    {
        public int RoundId { get; set; }
        public string Title { get; set; }
        public string? Description { get; set; }


        // Navigation Properties
        public virtual Round Round { get; set; }
        public virtual ICollection<Material> Materials { get; set; }
        public virtual ICollection<Progress> Progresses { get; set; }
    }
}
