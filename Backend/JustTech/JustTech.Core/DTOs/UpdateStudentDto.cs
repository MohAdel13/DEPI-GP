using System;
using System.Collections.Generic;
using System.Text;

namespace JustTech.Core.DTOs
{
    public class UpdateStudentDto
    {
        public string Name { get; set; }
        public string? Phone { get; set; }
        public string? Profession { get; set; }
        public string? Image { get; set; }
        public string? City { get; set; }
        public string? Country { get; set; }
        public string? StudentStatus { get; set; }
        public DateTime? Birthdate { get; set; }
    }
}
