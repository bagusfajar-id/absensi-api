using AbsensiApp.Application.DTOs;

namespace AbsensiApp.Application.Interfaces
{
    public interface IOfficeLocationService
    {
        Task<List<OfficeLocationResponseDto>> GetAllAsync();
        Task<OfficeLocationResponseDto> CreateAsync(OfficeLocationRequestDto request);
        Task<OfficeLocationResponseDto?> UpdateAsync(Guid id, OfficeLocationRequestDto request);
        Task<bool> DeleteAsync(Guid id);
    }
}