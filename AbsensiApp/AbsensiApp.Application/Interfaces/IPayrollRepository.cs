using AbsensiApp.Domain.Entities;

namespace AbsensiApp.Application.Interfaces
{
    public interface IPayrollRepository
    {
        Task<Payroll?> GetByEmployeeAndPeriodAsync(Guid employeeId, int month, int year);
        Task<List<Payroll>> GetAllByPeriodAsync(int month, int year);
        Task<Payroll?> GetByIdWithDetailsAsync(Guid id);
        Task<Payroll> CreateAsync(Payroll payroll);
        Task<Payroll> UpdateAsync(Payroll payroll);
    }
}