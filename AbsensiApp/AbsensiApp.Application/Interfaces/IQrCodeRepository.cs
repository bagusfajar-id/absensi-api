using AbsensiApp.Domain.Entities;

namespace AbsensiApp.Application.Interfaces
{
    public interface IQrCodeRepository
    {
        Task<QrCode?> GetValidCodeAsync(string code);
        Task<QrCode> CreateAsync(QrCode qrCode);
        Task DeleteExpiredAsync();
    }
}