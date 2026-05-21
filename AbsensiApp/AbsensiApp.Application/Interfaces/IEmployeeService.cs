using AbsensiApp.Application.DTOs;

namespace AbsensiApp.Application.Interfaces
{
    public interface IEmployeeService
    {
        Task<List<EmployeeDto>> GetAllAsync();
        Task<EmployeeDto?> GetByIdAsync(Guid id);
        Task<EmployeeDto> CreateAsync(EmployeeRequestDto request);
        Task<EmployeeDto?> UpdateAsync(Guid id, EmployeeUpdateDto request);
        Task<bool> DeleteAsync(Guid id);
    }
}