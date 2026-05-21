using AbsensiApp.Application.DTOs;

namespace AbsensiApp.Application.Interfaces
{
    public interface IDepartmentService
    {
        Task<List<DepartmentResponseDto>> GetAllAsync();
        Task<DepartmentResponseDto?> GetByIdAsync(Guid id);
        Task<DepartmentResponseDto> CreateAsync(DepartmentRequestDto request);
        Task<DepartmentResponseDto?> UpdateAsync(Guid id, DepartmentRequestDto request);
        Task<bool> DeleteAsync(Guid id);
    }
}