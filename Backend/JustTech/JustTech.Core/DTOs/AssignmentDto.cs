using System;
using System.Collections.Generic;
using System.Text;

namespace JustTech.Core.DTOs
{
    public class AssignmentDto
    {
        public int Id { get; set; }
        public int RoundId { get; set; }
        public string RoundName { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime? DueDate { get; set; }
        public int SubmissionsCount { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
