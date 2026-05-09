using AutoMapper;
using JustTech.Core.DTOs;
using JustTech.Core.Entities;

namespace JustTech.Business.MappingProfiles
{
    public class EnrollmentProfile : Profile
    {
        public EnrollmentProfile()
        {
            CreateMap<Enrollment, EnrollmentDto>()
                .ForMember(dest => dest.StudentName, opt => opt.MapFrom(src => src.Student != null ? src.Student.Name : null))
                .ForMember(dest => dest.RoundName, opt => opt.MapFrom(src => src.Round != null ? src.Round.Name : null))
                .ForMember(dest => dest.CourseName, opt => opt.MapFrom(src => src.Round != null && src.Round.Course != null ? src.Round.Course.Name : null));

            CreateMap<CreateEnrollmentDto, Enrollment>();
        }
    }
}
