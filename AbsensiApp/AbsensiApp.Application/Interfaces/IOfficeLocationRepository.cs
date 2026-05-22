using AbsensiApp.Domain.Entities;

namespace AbsensiApp.Application.Interfaces
{
    public interface IOfficeLocationRepository
    {
        Task<List<OfficeLocation>> GetAllAsync();
        Task<OfficeLocation?> GetActiveAsync();
        Task<OfficeLocation?> GetByIdAsync(Guid id);
        Task<OfficeLocation> CreateAsync(OfficeLocation location);
        Task<OfficeLocation> UpdateAsync(OfficeLocation location);
        Task<bool> DeleteAsync(Guid id);
    }
}