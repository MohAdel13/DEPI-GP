using System;
using System.Collections.Generic;
using System.Text;

namespace JustTech.Core.DTOs
{
    public class CertificateDto
    {
        public int Id { get; set; }
        public int StudentId { get; set; }
        public string StudentName { get; set; }
        public int RoundId { get; set; }
        public string RoundName { get; set; }
        public string CourseName { get; set; }
        public DateTime? IssuedAt { get; set; }
        public string? Url { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
