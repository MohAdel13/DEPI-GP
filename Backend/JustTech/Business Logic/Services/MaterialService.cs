using AutoMapper;
using JustTech.Core.DTOs;
using JustTech.Core.Entities;
using JustTech.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace JustTech.Business.Services
{
    public class MaterialService : IMaterialService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public MaterialService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IEnumerable<MaterialDto>> GetAllAsync()
        {
            var materials = await _unitOfWork.Materials.GetAllAsync();
            return _mapper.Map<IEnumerable<MaterialDto>>(materials);
        }
        public async Task<MaterialDto> GetByIdAsync(int id)
        {
            var material = await _unitOfWork.Materials.GetByIdAsync(id);
            if (material == null)
                throw new Exception($"Material with ID {id} not found");

            return _mapper.Map<MaterialDto>(material);
        }

        public async Task<IEnumerable<MaterialDto>> GetMaterialsByLectureIdAsync(int lectureId)
        {
            var lecture = await _unitOfWork.Lectures.GetByIdAsync(lectureId);
            if (lecture == null)
                throw new Exception($"Lecture with ID {lectureId} not found");

            var materials = await _unitOfWork.Materials.GetMaterialsByLectureIdAsync(lectureId);
            return _mapper.Map<IEnumerable<MaterialDto>>(materials);
        }

        public async Task<MaterialDto> CreateAsync(CreateMaterialDto createDto)
        {
            // Check if lecture exists
            var lecture = await _unitOfWork.Lectures.GetByIdAsync(createDto.LectureId);
            if (lecture == null)
                throw new Exception($"Lecture with ID {createDto.LectureId} not found");

            var material = _mapper.Map<Material>(createDto);
            var created = await _unitOfWork.Materials.AddAsync(material);
            await _unitOfWork.SaveChangesAsync();

            return _mapper.Map<MaterialDto>(created);
        }

        public async Task<MaterialDto> UpdateAsync(int id, UpdateMaterialDto updateDto)
        {
            var material = await _unitOfWork.Materials.GetByIdAsync(id);
            if (material == null)
                throw new Exception($"Material with ID {id} not found");

            _mapper.Map(updateDto, material);
            _unitOfWork.Materials.Update(material);
            await _unitOfWork.SaveChangesAsync();

            return _mapper.Map<MaterialDto>(material);
        }

        public async Task DeleteAsync(int id)
        {
            var material = await _unitOfWork.Materials.GetByIdAsync(id);
            if (material == null)
                throw new Exception($"Material with ID {id} not found");

            _unitOfWork.Materials.Delete(material);
            await _unitOfWork.SaveChangesAsync();
        }


    }
}
