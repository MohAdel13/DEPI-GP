using JustTech.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace JustTech.Core.Interfaces
{
    public interface IAssignmentRepository : IRepository<Assignment>
    {
        Task<IEnumerable<Assignment>> GetAssignmentsByRoundIdAsync(int roundId);
        Task<Assignment?> GetAssignmentWithSubmissionsAsync(int assignmentId);
    }
}
