using System;
using System.Collections.Generic;
using System.Text;

namespace JustTech.Core.DTOs
{
    public class SubmissionDto
    {
        public int Id { get; set; }
        public int AssignmentId { get; set; }
        public string AssignmentTitle { get; set; }
        public int StudentId { get; set; }
        public string StudentName { get; set; }
        public string? AnswerUrl { get; set; }
        public decimal? Grade { get; set; }
        public DateTime? SubmittedAt { get; set; }
        public string? Status { get; set; } // submitted / not submitted / late
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
