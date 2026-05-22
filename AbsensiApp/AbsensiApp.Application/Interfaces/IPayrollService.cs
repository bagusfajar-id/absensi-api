using AbsensiApp.Application.DTOs;

namespace AbsensiApp.Application.Interfaces
{
    public interface IPayrollService
    {
        Task<PayrollResponseDto> GeneratePayrollAsync(GeneratePayrollRequestDto request);
        Task<PayrollResponseDto?> GetByIdAsync(Guid id);
        Task<List<PayrollResponseDto>> GetAllByPeriodAsync(int month, int year);
        Task<PayrollResponseDto?> ApprovePayrollAsync(Guid id);
        Task<PayrollResponseDto?> GetMyPayrollAsync(Guid employeeId, int month, int year);
    }
}