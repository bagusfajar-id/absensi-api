using AbsensiApp.Application.DTOs;
using AbsensiApp.Application.Interfaces;
using AbsensiApp.Domain.Entities;

namespace AbsensiApp.Infrastructure.Services
{
    public class DepartmentService : IDepartmentService
    {
        private readonly IDepartmentRepository _departmentRepository;

        public DepartmentService(IDepartmentRepository departmentRepository)
        {
            _departmentRepository = departmentRepository;
        }

        public async Task<List<DepartmentResponseDto>> GetAllAsync()
        {
            var departments = await _departmentRepository.GetAllAsync();
            return departments.Select(d => new DepartmentResponseDto
            {
                Id = d.Id,
                Name = d.Name,
                Description = d.Description,
                CreatedAt = d.CreatedAt
            }).ToList();
        }

        public async Task<DepartmentResponseDto?> GetByIdAsync(Guid id)
        {
            var department = await _departmentRepository.GetByIdAsync(id);
            if (department == null) return null;

            return new DepartmentResponseDto
            {
                Id = department.Id,
                Name = department.Name,
                Description = department.Description,
                CreatedAt = department.CreatedAt
            };
        }

        public async Task<DepartmentResponseDto> CreateAsync(DepartmentRequestDto request)
        {
            var department = new Department
            {
                Id = Guid.NewGuid(),
                Name = request.Name,
                Description = request.Description,
                CreatedAt = DateTime.UtcNow
            };

            await _departmentRepository.CreateAsync(department);

            return new DepartmentResponseDto
            {
                Id = department.Id,
                Name = department.Name,
                Description = department.Description,
                CreatedAt = department.CreatedAt
            };
        }

        public async Task<DepartmentResponseDto?> UpdateAsync(Guid id, DepartmentRequestDto request)
        {
            var department = await _departmentRepository.GetByIdAsync(id);
            if (department == null) return null;

            department.Name = request.Name;
            department.Description = request.Description;

            await _departmentRepository.UpdateAsync(department);

            return new DepartmentResponseDto
            {
                Id = department.Id,
                Name = department.Name,
                Description = department.Description,
                CreatedAt = department.CreatedAt
            };
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            return await _departmentRepository.DeleteAsync(id);
        }
    }
}