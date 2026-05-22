using AbsensiApp.Application.Interfaces;
using AbsensiApp.Domain.Entities;
using AbsensiApp.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace AbsensiApp.Infrastructure.Repositories
{
    public class AttendanceRepository : IAttendanceRepository
    {
        private readonly AppDbContext _context;

        public AttendanceRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Attendance?> GetByEmployeeAndDateAsync(Guid employeeId, DateTime date)
            => await _context.Attendances
                .Include(a => a.Employee)
                .FirstOrDefaultAsync(a => a.EmployeeId == employeeId && a.Date == date.Date);

        public async Task<List<Attendance>> GetByEmployeeIdAsync(Guid employeeId)
            => await _context.Attendances
                .Include(a => a.Employee)
                .Where(a => a.EmployeeId == employeeId)
                .OrderByDescending(a => a.Date)
                .ToListAsync();

        public async Task<List<Attendance>> GetAllByDateAsync(DateTime? date)
        {
            var query = _context.Attendances
                .Include(a => a.Employee)
                .AsQueryable();

            if (date.HasValue)
                query = query.Where(a => a.Date == date.Value.Date);

            return await query.OrderByDescending(a => a.Date).ToListAsync();
        }

        public async Task<Attendance> CreateAsync(Attendance attendance)
        {
            _context.Attendances.Add(attendance);
            await _context.SaveChangesAsync();
            return attendance;
        }

        public async Task<Attendance> UpdateAsync(Attendance attendance)
        {
            _context.Attendances.Update(attendance);
            await _context.SaveChangesAsync();
            return attendance;
        }
    }
}