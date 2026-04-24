using AutoMapper;
using JustTech.Core.DTOs;
using JustTech.Core.Entities;

namespace JustTech.Business.MappingProfiles
{
    public class CourseProfile : Profile
    {
        public CourseProfile()
        {
            CreateMap<Course, CourseDto>(); // Convert Entity to DTO
            CreateMap<CreateCourseDto, Course>(); // Convert DTO to Entity
            CreateMap<UpdateCourseDto, Course>(); // Convert DTO to existing DTO
        }
    }
}
