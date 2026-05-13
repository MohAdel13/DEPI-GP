using AutoMapper;
using Business.Logic.Services;
using JustTech.Core.DTOs;
using JustTech.Core.Entities;
using JustTech.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace JustTech.Business.Services
{
    public class CourseService : ICourseService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public CourseService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task<IEnumerable<CourseDto>> GetAllAsync()
        {
            var courses = await _unitOfWork.Courses.GetAllAsync();
            return _mapper.Map<IEnumerable<CourseDto>>(courses);
        }

        public async Task<CourseDto?> GetByIdAsync(int id)
        {
            var course = await _unitOfWork.Courses.GetByIdAsync(id);
            return course == null ? null : _mapper.Map<CourseDto>(course);
        }


        public async Task<CourseDto> CreateAsync(CreateCourseDto courseDto)
        {
            var course = _mapper.Map<Course>(courseDto);
            var created = await _unitOfWork.Courses.AddAsync(course);

            await _unitOfWork.SaveChangesAsync();

            return _mapper.Map<CourseDto>(created);
        }
        public async Task<CourseDto?> UpdateAsync(int id, UpdateCourseDto updateDto)
        {
            var course = await _unitOfWork.Courses.GetByIdAsync(id);
            if (course == null)
                return null;

            _mapper.Map(updateDto, course);
            _unitOfWork.Courses.Update(course);
            await _unitOfWork.SaveChangesAsync();

            return _mapper.Map<CourseDto>(course);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var course = await _unitOfWork.Courses.GetByIdAsync(id);
            if (course == null)
                return false;

            _unitOfWork.Courses.Delete(course);
            await _unitOfWork.SaveChangesAsync();
            return true;
        }
    



    }
}

