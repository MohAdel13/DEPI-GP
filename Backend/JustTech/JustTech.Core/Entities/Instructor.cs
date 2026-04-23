using System;
using System.Collections.Generic;
using System.Text;

namespace JustTech.Core.Entities
{
    public class Instructor : BaseEntity
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string? Phone { get; set; }
        public string? Profession { get; set; }
        public string? City { get; set; }
        public string? Country { get; set; }
        public DateTime? BirthDate { get; set; }

        // Navigation Property
        public virtual ICollection<Round> Rounds { get; set; }
    }
}
