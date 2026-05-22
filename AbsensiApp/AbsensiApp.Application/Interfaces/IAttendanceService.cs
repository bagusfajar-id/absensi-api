using AbsensiApp.Application.DTOs;

namespace AbsensiApp.Application.Interfaces
{
    public interface IAttendanceService
    {
        Task<QrCodeResponseDto> GenerateQrCodeAsync();
        Task<AttendanceResponseDto> CheckInQrAsync(Guid employeeId, CheckInQrRequestDto request);
        Task<AttendanceResponseDto> CheckInGpsAsync(Guid employeeId, CheckInGpsRequestDto request);
        Task<AttendanceResponseDto> CheckOutAsync(Guid employeeId, CheckOutRequestDto request);
        Task<List<AttendanceResponseDto>> GetMyAttendanceAsync(Guid employeeId);
        Task<List<AttendanceResponseDto>> GetAllAttendanceAsync(DateTime? date);
    }
}