using AutoMapper;
using JustTech.Core.DTOs;
using JustTech.Core.Entities;

namespace JustTech.Business.MappingProfiles
{
    public class StudentProfile : Profile
    {
        public StudentProfile()
        {
            // Auth Mapping
            CreateMap<StudentRegisterDto, Student>();
            CreateMap<Student, StudentAuthResponseDto>();

            // CRUD mapping
            CreateMap<Student, StudentDto>();
            CreateMap<CreateStudentDto, Student>();
            CreateMap<UpdateStudentDto, Student>();
        }
    }
}
