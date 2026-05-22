using AbsensiApp.Application.Interfaces;
using AbsensiApp.Domain.Entities;
using AbsensiApp.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace AbsensiApp.Infrastructure.Repositories
{
    public class OfficeLocationRepository : IOfficeLocationRepository
    {
        private readonly AppDbContext _context;

        public OfficeLocationRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<OfficeLocation>> GetAllAsync()
            => await _context.OfficeLocations.ToListAsync();

        public async Task<OfficeLocation?> GetActiveAsync()
            => await _context.OfficeLocations.FirstOrDefaultAsync(o => o.IsActive);

        public async Task<OfficeLocation?> GetByIdAsync(Guid id)
            => await _context.OfficeLocations.FindAsync(id);

        public async Task<OfficeLocation> CreateAsync(OfficeLocation location)
        {
            _context.OfficeLocations.Add(location);
            await _context.SaveChangesAsync();
            return location;
        }

        public async Task<OfficeLocation> UpdateAsync(OfficeLocation location)
        {
            _context.OfficeLocations.Update(location);
            await _context.SaveChangesAsync();
            return location;
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var location = await _context.OfficeLocations.FindAsync(id);
            if (location == null) return false;
            _context.OfficeLocations.Remove(location);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}