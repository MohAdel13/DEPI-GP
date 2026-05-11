using AutoMapper;
using JustTech.Core.DTOs;
using JustTech.Core.Entities;

namespace JustTech.Business.MappingProfiles
{
    public class AssignmentProfile : Profile
    {
        public AssignmentProfile()
        {
            CreateMap<Assignment, AssignmentDto>()
                    .ForMember(dest => dest.RoundName, opt => opt.MapFrom(src => src.Round != null ? src.Round.Name : null))
                    .ForMember(dest => dest.SubmissionsCount, opt => opt.MapFrom(src => src.Submissions != null ? src.Submissions.Count(s => s.DeletedAt == null) : 0));

            CreateMap<CreateAssignmentDto, Assignment>();
            CreateMap<UpdateAssignmentDto, Assignment>();
        }
    }
}
