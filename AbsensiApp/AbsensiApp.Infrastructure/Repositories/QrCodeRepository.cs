using AbsensiApp.Application.Interfaces;
using AbsensiApp.Domain.Entities;
using AbsensiApp.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace AbsensiApp.Infrastructure.Repositories
{
    public class QrCodeRepository : IQrCodeRepository
    {
        private readonly AppDbContext _context;

        public QrCodeRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<QrCode?> GetValidCodeAsync(string code)
            => await _context.QrCodes
                .FirstOrDefaultAsync(q => q.Code == code && q.ExpiredAt > DateTime.UtcNow);

        public async Task<QrCode> CreateAsync(QrCode qrCode)
        {
            _context.QrCodes.Add(qrCode);
            await _context.SaveChangesAsync();
            return qrCode;
        }

        public async Task DeleteExpiredAsync()
        {
            var expired = await _context.QrCodes
                .Where(q => q.ExpiredAt < DateTime.UtcNow)
                .ToListAsync();
            _context.QrCodes.RemoveRange(expired);
            await _context.SaveChangesAsync();
        }
    }
}