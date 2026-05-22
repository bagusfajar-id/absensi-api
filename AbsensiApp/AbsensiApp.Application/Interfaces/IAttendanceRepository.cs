using AbsensiApp.Domain.Entities;

namespace AbsensiApp.Application.Interfaces
{
    public interface IAttendanceRepository
    {
        Task<Attendance?> GetByEmployeeAndDateAsync(Guid employeeId, DateTime date);
        Task<List<Attendance>> GetByEmployeeIdAsync(Guid employeeId);
        Task<List<Attendance>> GetAllByDateAsync(DateTime? date);
        Task<Attendance> CreateAsync(Attendance attendance);
        Task<Attendance> UpdateAsync(Attendance attendance);
    }
}