using AutoMapper;
using JustTech.Core.DTOs;
using JustTech.Core.Entities;

namespace JustTech.Business.MappingProfiles
{
    public class LectureProfile : Profile
    {
        public LectureProfile()
        {
            CreateMap<Lecture, LectureDto>()
                .ForMember(dest => dest.RoundName, opt => opt.MapFrom(src => src.Round != null ? src.Round.Name : null))
                .ForMember(dest => dest.MaterialsCount, opt => opt.MapFrom(src => src.Materials != null ? src.Materials.Count(m => m.DeletedAt == null) : 0));

            CreateMap<CreateLectureDto, Lecture>();
            CreateMap<UpdateLectureDto, Lecture>();
        }
    }
}
