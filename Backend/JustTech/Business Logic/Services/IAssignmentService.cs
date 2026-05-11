using JustTech.Core.DTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace JustTech.Business.Services
{
   public interface IAssignmentService
    {
        Task<IEnumerable<AssignmentDto>> GetAllAsync();
        Task<AssignmentDto?> GetByIdAsync(int id);
        Task<IEnumerable<AssignmentDto>> GetAssignmentsByRoundIdAsync(int roundId);
        Task<AssignmentDto> CreateAsync(CreateAssignmentDto createDto);
        Task<AssignmentDto?> UpdateAsync(int id, UpdateAssignmentDto updateDto);
        Task<bool> DeleteAsync(int id);
    }
}
