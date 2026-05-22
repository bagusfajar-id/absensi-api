using AbsensiApp.Application.DTOs;
using AbsensiApp.Application.Interfaces;
using AbsensiApp.Domain.Entities;

namespace AbsensiApp.Infrastructure.Services
{
    public class OfficeLocationService : IOfficeLocationService
    {
        private readonly IOfficeLocationRepository _officeLocationRepository;

        public OfficeLocationService(IOfficeLocationRepository officeLocationRepository)
        {
            _officeLocationRepository = officeLocationRepository;
        }

        public async Task<List<OfficeLocationResponseDto>> GetAllAsync()
        {
            var locations = await _officeLocationRepository.GetAllAsync();
            return locations.Select(o => new OfficeLocationResponseDto
            {
                Id = o.Id,
                Name = o.Name,
                Latitude = o.Latitude,
                Longitude = o.Longitude,
                RadiusInMeters = o.RadiusInMeters,
                IsActive = o.IsActive
            }).ToList();
        }

        public async Task<OfficeLocationResponseDto> CreateAsync(OfficeLocationRequestDto request)
        {
            var location = new OfficeLocation
            {
                Id = Guid.NewGuid(),
                Name = request.Name,
                Latitude = request.Latitude,
                Longitude = request.Longitude,
                RadiusInMeters = request.RadiusInMeters,
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            };

            await _officeLocationRepository.CreateAsync(location);

            return new OfficeLocationResponseDto
            {
                Id = location.Id,
                Name = location.Name,
                Latitude = location.Latitude,
                Longitude = location.Longitude,
                RadiusInMeters = location.RadiusInMeters,
                IsActive = location.IsActive
            };
        }

        public async Task<OfficeLocationResponseDto?> UpdateAsync(Guid id, OfficeLocationRequestDto request)
        {
            var location = await _officeLocationRepository.GetByIdAsync(id);
            if (location == null) return null;

            location.Name = request.Name;
            location.Latitude = request.Latitude;
            location.Longitude = request.Longitude;
            location.RadiusInMeters = request.RadiusInMeters;

            await _officeLocationRepository.UpdateAsync(location);

            return new OfficeLocationResponseDto
            {
                Id = location.Id,
                Name = location.Name,
                Latitude = location.Latitude,
                Longitude = location.Longitude,
                RadiusInMeters = location.RadiusInMeters,
                IsActive = location.IsActive
            };
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            return await _officeLocationRepository.DeleteAsync(id);
        }
    }
}