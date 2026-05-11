using AutoMapper;
using JustTech.Core.DTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace JustTech.Business.MappingProfiles
{
    public class ProgressProfile : Profile
    {
        public ProgressProfile()
        {
            CreateMap<JustTech.Core.Entities.Progress, ProgressDto>()
                    .ForMember(dest => dest.StudentName, opt => opt.MapFrom(src => src.Student != null ? src.Student.Name : null))
                    .ForMember(dest => dest.LectureTitle, opt => opt.MapFrom(src => src.Lecture != null ? src.Lecture.Title : null))
                    .ForMember(dest => dest.RoundName, opt => opt.MapFrom(src => src.Lecture != null && src.Lecture.Round != null ? src.Lecture.Round.Name : null));
        }
    }
}
