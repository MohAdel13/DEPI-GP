using AutoMapper;
using JustTech.Core.DTOs;
using JustTech.Core.Entities;

namespace JustTech.Business.MappingProfiles
{
    public class RoundProfile : Profile
    {
        public RoundProfile()
        {
            CreateMap<Round, RoundDto>()
                .ForMember(dest => dest.CourseName, opt => opt.MapFrom(src => src.Course != null ? src.Course.Name : null))
                .ForMember(dest => dest.InstructorName, opt => opt.MapFrom(src => src.Instructor != null ? src.Instructor.Name : null))
                .ForMember(dest => dest.EnrollmentsCount, opt => opt.MapFrom(src => src.Enrollments != null ? src.Enrollments.Count(e => e.DeletedAt == null) : 0));


            CreateMap<CreateRoundDto, Round>();
            CreateMap<UpdateRoundDto, Round>();
        }
    }
}
