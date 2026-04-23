using System;
using System.Collections.Generic;
using System.Text;

namespace JustTech.Core.Entities
{
    public class Material : BaseEntity
    {
        public int LectureId { get; set; }
        public string Url { get; set; }
        public string Type { get; set; } // Video , Pdf, Document , etc

        // Navigation Properties
        public virtual Lecture Lecture { get; set; }

    }
}
