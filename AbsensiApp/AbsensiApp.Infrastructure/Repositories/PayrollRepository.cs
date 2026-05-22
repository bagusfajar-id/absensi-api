using AbsensiApp.Application.Interfaces;
using AbsensiApp.Domain.Entities;
using AbsensiApp.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace AbsensiApp.Infrastructure.Repositories
{
    public class PayrollRepository : IPayrollRepository
    {
        private readonly AppDbContext _context;

        public PayrollRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Payroll?> GetByEmployeeAndPeriodAsync(Guid employeeId, int month, int year)
            => await _context.Payrolls
                .Include(p => p.Employee)
                .Include(p => p.PayrollDetails)
                .FirstOrDefaultAsync(p => p.EmployeeId == employeeId
                    && p.Month == month && p.Year == year);

        public async Task<List<Payroll>> GetAllByPeriodAsync(int month, int year)
            => await _context.Payrolls
                .Include(p => p.Employee)
                .Include(p => p.PayrollDetails)
                .Where(p => p.Month == month && p.Year == year)
                .ToListAsync();

        public async Task<Payroll?> GetByIdWithDetailsAsync(Guid id)
            => await _context.Payrolls
                .Include(p => p.Employee)
                .Include(p => p.PayrollDetails)
                .FirstOrDefaultAsync(p => p.Id == id);

        public async Task<Payroll> CreateAsync(Payroll payroll)
        {
            _context.Payrolls.Add(payroll);
            await _context.SaveChangesAsync();
            return payroll;
        }

        public async Task<Payroll> UpdateAsync(Payroll payroll)
        {
            _context.Payrolls.Update(payroll);
            await _context.SaveChangesAsync();
            return payroll;
        }
    }
}