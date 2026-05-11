using AutoMapper;
using JustTech.Core.DTOs;
using JustTech.Core.Entities;
using JustTech.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace JustTech.Business.Services
{
    public class CertificateService : ICertificateService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public CertificateService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IEnumerable<CertificateDto>> GetAllAsync()
        {
            var certificates = await _unitOfWork.Certificates.GetAllAsync();
            return _mapper.Map<IEnumerable<CertificateDto>>(certificates);
        }

        public async Task<CertificateDto?> GetByIdAsync(int id)
        {
            var certificate = await _unitOfWork.Certificates.GetByIdAsync(id);
            return certificate == null ? null : _mapper.Map<CertificateDto>(certificate);
        }

        public async Task<IEnumerable<CertificateDto>> GetCertificatesByStudentIdAsync(int studentId)
        {
            var student = await _unitOfWork.Students.GetByIdAsync(studentId);
            if (student == null)
                return new List<CertificateDto>();

            var certificates = await _unitOfWork.Certificates.GetCertificatesByStudentIdAsync(studentId);
            return _mapper.Map<IEnumerable<CertificateDto>>(certificates);
        }

        public async Task<IEnumerable<CertificateDto>> GetCertificatesByRoundIdAsync(int roundId)
        {
            var round = await _unitOfWork.Rounds.GetByIdAsync(roundId);
            if (round == null)
                return new List<CertificateDto>();

            var certificates = await _unitOfWork.Certificates.GetCertificatesByRoundIdAsync(roundId);
            return _mapper.Map<IEnumerable<CertificateDto>>(certificates);
        }

        public async Task<CertificateDto?> GetCertificateByStudentAndRoundAsync(int studentId, int roundId)
        {
            var certificate = await _unitOfWork.Certificates.GetCertificateByStudentAndRoundAsync(studentId, roundId);
            return certificate == null ? null : _mapper.Map<CertificateDto>(certificate);
        }

        public async Task<CertificateDto?> GenerateCertificateAsync(GenerateCertificateDto generateDto)
        {
            // Check if student exists
            var student = await _unitOfWork.Students.GetByIdAsync(generateDto.StudentId);
            if (student == null)
                return null;

            // Check if round exists
            var round = await _unitOfWork.Rounds.GetByIdAsync(generateDto.RoundId);
            if (round == null)
                return null;
            // Check if already has certificate
            var existingCertificate = await _unitOfWork.Certificates.GetCertificateByStudentAndRoundAsync(generateDto.StudentId, generateDto.RoundId);
            if (existingCertificate != null)
                return _mapper.Map<CertificateDto>(existingCertificate);

            // Check if student completed the round (progress >= 100%)
            var progressPercentage = await _unitOfWork.Progresses.GetStudentProgressPercentageAsync(generateDto.StudentId, generateDto.RoundId);
            if (progressPercentage < 100)
                return null;

            // Generate certificate
            var certificate = new Certificate
            {
                StudentId = generateDto.StudentId,
                RoundId = generateDto.RoundId,
                IssuedAt = DateTime.UtcNow,
                Url = $"https://api.justtech.com/certificates/{generateDto.StudentId}/{generateDto.RoundId}"
            };

            var created = await _unitOfWork.Certificates.AddAsync(certificate);
            await _unitOfWork.SaveChangesAsync();

            return _mapper.Map<CertificateDto>(created);
        }

        public async Task<CertificateDto?> UpdateCertificateAsync(int id, UpdateCertificateDto updateDto)
        {
            var certificate = await _unitOfWork.Certificates.GetByIdAsync(id);
            if (certificate == null)
                return null;

            certificate.Url = updateDto.Url ?? certificate.Url;
            _unitOfWork.Certificates.Update(certificate);
            await _unitOfWork.SaveChangesAsync();

            return _mapper.Map<CertificateDto>(certificate);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var certificate = await _unitOfWork.Certificates.GetByIdAsync(id);
            if (certificate == null)
                return false;

            _unitOfWork.Certificates.Delete(certificate);
            await _unitOfWork.SaveChangesAsync();
            return true;
        }

        public async Task<bool> HasCertificateAsync(int studentId, int roundId)
        {
            return await _unitOfWork.Certificates.HasCertificateAsync(studentId, roundId);
        }


    }
}
