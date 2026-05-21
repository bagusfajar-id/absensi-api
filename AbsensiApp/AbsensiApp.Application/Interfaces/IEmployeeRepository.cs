using AbsensiApp.Domain.Entities;

namespace AbsensiApp.Application.Interfaces
{
    public interface IEmployeeRepository : IGenericRepository<Employee>
    {
        Task<Employee?> GetByEmailAsync(string email);
        Task<List<Employee>> GetAllWithDepartmentAsync();
        Task<Employee?> GetByIdWithDepartmentAsync(Guid id);
    }
}