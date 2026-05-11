using AutoMapper;
using JustTech.Core.DTOs;
using JustTech.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace JustTech.Business.MappingProfiles
{
    public class SubmissionProfile : Profile
    {
        public SubmissionProfile()
        {
            CreateMap<Submission, SubmissionDto>()
                .ForMember(dest => dest.AssignmentTitle, opt => opt.MapFrom(src => src.Assignment != null ? src.Assignment.Title : null))
                .ForMember(dest => dest.StudentName, opt => opt.MapFrom(src => src.Student != null ? src.Student.Name : null));
        }
    }
}
