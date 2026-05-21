using AbsensiApp.Application.Interfaces;
using AbsensiApp.Domain.Entities;
using AbsensiApp.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace AbsensiApp.Infrastructure.Repositories
{
    public class EmployeeRepository : IEmployeeRepository
    {
        private readonly AppDbContext _context;

        public EmployeeRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<Employee>> GetAllAsync()
            => await _context.Employees.ToListAsync();

        public async Task<List<Employee>> GetAllWithDepartmentAsync()
            => await _context.Employees.Include(e => e.Department).ToListAsync();

        public async Task<Employee?> GetByIdAsync(Guid id)
            => await _context.Employees.FindAsync(id);

        public async Task<Employee?> GetByIdWithDepartmentAsync(Guid id)
            => await _context.Employees.Include(e => e.Department)
                .FirstOrDefaultAsync(e => e.Id == id);

        public async Task<Employee?> GetByEmailAsync(string email)
            => await _context.Employees.Include(e => e.Department)
                .FirstOrDefaultAsync(e => e.Email == email);

        public async Task<Employee> CreateAsync(Employee entity)
        {
            _context.Employees.Add(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task<Employee> UpdateAsync(Employee entity)
        {
            _context.Employees.Update(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var employee = await _context.Employees.FindAsync(id);
            if (employee == null) return false;
            _context.Employees.Remove(employee);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}