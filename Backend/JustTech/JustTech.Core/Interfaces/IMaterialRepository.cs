using JustTech.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace JustTech.Core.Interfaces
{
    public interface IMaterialRepository : IRepository<Material>
    {
        Task<IEnumerable<Material>> GetMaterialsByLectureIdAsync(int lectureId);
    }
}
