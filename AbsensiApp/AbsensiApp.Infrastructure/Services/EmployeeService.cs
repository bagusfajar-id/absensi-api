using AbsensiApp.Application.DTOs;
using AbsensiApp.Application.Interfaces;
using AbsensiApp.Domain.Entities;

namespace AbsensiApp.Infrastructure.Services
{
    public class EmployeeService : IEmployeeService
    {
        private readonly IEmployeeRepository _employeeRepository;

        public EmployeeService(IEmployeeRepository employeeRepository)
        {
            _employeeRepository = employeeRepository;
        }

        public async Task<List<EmployeeDto>> GetAllAsync()
        {
            var employees = await _employeeRepository.GetAllWithDepartmentAsync();
            return employees.Select(e => new EmployeeDto
            {
                Id = e.Id,
                EmployeeCode = e.EmployeeCode,
                FullName = e.FullName,
                Email = e.Email,
                Role = e.Role,
                PhoneNumber = e.PhoneNumber,
                Address = e.Address,
                BasicSalary = e.BasicSalary,
                JoinDate = e.JoinDate,
                IsActive = e.IsActive,
                DepartmentName = e.Department?.Name ?? ""
            }).ToList();
        }

        public async Task<EmployeeDto?> GetByIdAsync(Guid id)
        {
            var e = await _employeeRepository.GetByIdWithDepartmentAsync(id);
            if (e == null) return null;

            return new EmployeeDto
            {
                Id = e.Id,
                EmployeeCode = e.EmployeeCode,
                FullName = e.FullName,
                Email = e.Email,
                Role = e.Role,
                PhoneNumber = e.PhoneNumber,
                Address = e.Address,
                BasicSalary = e.BasicSalary,
                JoinDate = e.JoinDate,
                IsActive = e.IsActive,
                DepartmentName = e.Department?.Name ?? ""
            };
        }

        public async Task<EmployeeDto> CreateAsync(EmployeeRequestDto request)
        {
            var employee = new Employee
            {
                Id = Guid.NewGuid(),
                EmployeeCode = request.EmployeeCode,
                FullName = request.FullName,
                Email = request.Email,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password),
                Role = request.Role,
                PhoneNumber = request.PhoneNumber,
                Address = request.Address,
                BasicSalary = request.BasicSalary,
                JoinDate = request.JoinDate,
                IsActive = true,
                DepartmentId = request.DepartmentId,
                CreatedAt = DateTime.UtcNow
            };

            await _employeeRepository.CreateAsync(employee);

            var created = await _employeeRepository.GetByIdWithDepartmentAsync(employee.Id);

            return new EmployeeDto
            {
                Id = created!.Id,
                EmployeeCode = created.EmployeeCode,
                FullName = created.FullName,
                Email = created.Email,
                Role = created.Role,
                PhoneNumber = created.PhoneNumber,
                Address = created.Address,
                BasicSalary = created.BasicSalary,
                JoinDate = created.JoinDate,
                IsActive = created.IsActive,
                DepartmentName = created.Department?.Name ?? ""
            };
        }

        public async Task<EmployeeDto?> UpdateAsync(Guid id, EmployeeUpdateDto request)
        {
            var employee = await _employeeRepository.GetByIdWithDepartmentAsync(id);
            if (employee == null) return null;

            employee.FullName = request.FullName;
            employee.PhoneNumber = request.PhoneNumber;
            employee.Address = request.Address;
            employee.BasicSalary = request.BasicSalary;
            employee.Role = request.Role;
            employee.DepartmentId = request.DepartmentId;
            employee.IsActive = request.IsActive;

            await _employeeRepository.UpdateAsync(employee);

            var updated = await _employeeRepository.GetByIdWithDepartmentAsync(id);

            return new EmployeeDto
            {
                Id = updated!.Id,
                EmployeeCode = updated.EmployeeCode,
                FullName = updated.FullName,
                Email = updated.Email,
                Role = updated.Role,
                PhoneNumber = updated.PhoneNumber,
                Address = updated.Address,
                BasicSalary = updated.BasicSalary,
                JoinDate = updated.JoinDate,
                IsActive = updated.IsActive,
                DepartmentName = updated.Department?.Name ?? ""
            };
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            return await _employeeRepository.DeleteAsync(id);
        }
    }
}