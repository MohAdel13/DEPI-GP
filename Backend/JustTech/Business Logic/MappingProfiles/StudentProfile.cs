using AutoMapper;
using JustTech.Core.DTOs;
using JustTech.Core.Entities;

namespace JustTech.Business.MappingProfiles
{
    public class StudentProfile : Profile
    {
        public StudentProfile()
        {
            CreateMap<StudentRegisterDto, Student>();
            CreateMap<Student, StudentAuthResponseDto>();
        }
    }
}
