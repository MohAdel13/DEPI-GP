using System;
using System.Collections.Generic;
using System.Text;

namespace JustTech.Core.Entities
{
    public class Progress : BaseEntity
    {
        public int StudentId { get; set; }
        public int LectureId { get; set; }
        public bool IsCompleted { get; set; }
        public DateTime? CompletedAt { get; set; }
        public DateTime LastWatchedAt { get; set; }


        // Navigation Properties
        public virtual Student Student { get; set; }
        public virtual Lecture Lecture { get; set; }
    }
}
