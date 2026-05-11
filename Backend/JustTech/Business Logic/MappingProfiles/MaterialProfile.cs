using AutoMapper;
using JustTech.Core.DTOs;
using JustTech.Core.Entities;

namespace JustTech.Business.MappingProfiles
{
    public class MaterialProfile : Profile
    {
        public MaterialProfile()
        {
            CreateMap<Material, MaterialDto>()
                   .ForMember(dest => dest.LectureTitle, opt => opt.MapFrom(src => src.Lecture != null ? src.Lecture.Title : null));

            CreateMap<CreateMaterialDto, Material>();
            CreateMap<UpdateMaterialDto, Material>();
        }
    }
}
