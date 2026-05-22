using AbsensiApp.Application.DTOs;
using AbsensiApp.Application.Interfaces;
using AbsensiApp.Domain.Entities;

namespace AbsensiApp.Infrastructure.Services
{
    public class PayrollService : IPayrollService
    {
        private readonly IPayrollRepository _payrollRepository;
        private readonly IAttendanceRepository _attendanceRepository;
        private readonly IEmployeeRepository _employeeRepository;

        public PayrollService(
            IPayrollRepository payrollRepository,
            IAttendanceRepository attendanceRepository,
            IEmployeeRepository employeeRepository)
        {
            _payrollRepository = payrollRepository;
            _attendanceRepository = attendanceRepository;
            _employeeRepository = employeeRepository;
        }

        public async Task<PayrollResponseDto> GeneratePayrollAsync(GeneratePayrollRequestDto request)
        {
            // Cek sudah digenerate belum
            var existing = await _payrollRepository
                .GetByEmployeeAndPeriodAsync(request.EmployeeId, request.Month, request.Year);
            if (existing != null)
                throw new InvalidOperationException("Payroll bulan ini sudah digenerate!");

            var employee = await _employeeRepository.GetByIdWithDepartmentAsync(request.EmployeeId);
            if (employee == null)
                throw new InvalidOperationException("Employee tidak ditemukan!");

            // Ambil data absensi bulan ini
            var attendances = await _attendanceRepository.GetByEmployeeIdAsync(request.EmployeeId);
            var monthAttendances = attendances
                .Where(a => a.Date.Month == request.Month && a.Date.Year == request.Year)
                .ToList();

            // Hitung hari kerja (weekdays)
            var workingDays = GetWorkingDays(request.Month, request.Year);
            var presentDays = monthAttendances.Count(a => a.Status == "Present" || a.Status == "Late");
            var lateDays = monthAttendances.Count(a => a.Status == "Late");
            var absentDays = workingDays - presentDays;

            // Kalkulasi gaji
            var basicSalary = employee.BasicSalary;
            var dailySalary = basicSalary / workingDays;

            // Potongan
            var absentDeduction = dailySalary * absentDays;
            var lateDeduction = 50000 * lateDays; // Rp 50.000 per telat
            var totalDeduction = absentDeduction + lateDeduction;

            // Lembur (1.5x gaji per jam)
            var hourlyRate = basicSalary / (workingDays * 8);
            var overtimePay = hourlyRate * 1.5m * request.OvertimeHours;

            // Tunjangan transport
            var transportAllowance = 500000m;
            var totalAllowance = transportAllowance;

            // Gaji bersih
            var netSalary = basicSalary + totalAllowance + overtimePay - totalDeduction;

            var details = new List<PayrollDetail>
            {
                new() { Id = Guid.NewGuid(), Type = "Allowance", Description = "Tunjangan Transport", Amount = transportAllowance },
                new() { Id = Guid.NewGuid(), Type = "Deduction", Description = $"Potongan Alpha ({absentDays} hari)", Amount = absentDeduction },
                new() { Id = Guid.NewGuid(), Type = "Deduction", Description = $"Potongan Telat ({lateDays} hari)", Amount = lateDeduction },
                new() { Id = Guid.NewGuid(), Type = "Overtime", Description = $"Lembur ({request.OvertimeHours} jam)", Amount = overtimePay }
            };

            var payroll = new Payroll
            {
                Id = Guid.NewGuid(),
                EmployeeId = request.EmployeeId,
                Month = request.Month,
                Year = request.Year,
                BasicSalary = basicSalary,
                TotalAllowance = totalAllowance,
                TotalDeduction = totalDeduction,
                OvertimePay = overtimePay,
                NetSalary = netSalary,
                WorkingDays = workingDays,
                PresentDays = presentDays,
                LateDays = lateDays,
                AbsentDays = absentDays,
                OvertimeHours = request.OvertimeHours,
                Status = "Draft",
                CreatedAt = DateTime.UtcNow,
                PayrollDetails = details
            };

            await _payrollRepository.CreateAsync(payroll);

            return MapToDto(payroll, employee);
        }

        public async Task<PayrollResponseDto?> GetByIdAsync(Guid id)
        {
            var payroll = await _payrollRepository.GetByIdWithDetailsAsync(id);
            if (payroll == null) return null;
            return MapToDto(payroll, payroll.Employee);
        }

        public async Task<List<PayrollResponseDto>> GetAllByPeriodAsync(int month, int year)
        {
            var payrolls = await _payrollRepository.GetAllByPeriodAsync(month, year);
            return payrolls.Select(p => MapToDto(p, p.Employee)).ToList();
        }

        public async Task<PayrollResponseDto?> ApprovePayrollAsync(Guid id)
        {
            var payroll = await _payrollRepository.GetByIdWithDetailsAsync(id);
            if (payroll == null) return null;

            payroll.Status = "Approved";
            await _payrollRepository.UpdateAsync(payroll);

            return MapToDto(payroll, payroll.Employee);
        }

        public async Task<PayrollResponseDto?> GetMyPayrollAsync(Guid employeeId, int month, int year)
        {
            var payroll = await _payrollRepository
                .GetByEmployeeAndPeriodAsync(employeeId, month, year);
            if (payroll == null) return null;
            return MapToDto(payroll, payroll.Employee);
        }

        private int GetWorkingDays(int month, int year)
        {
            var days = 0;
            var daysInMonth = DateTime.DaysInMonth(year, month);
            for (var day = 1; day <= daysInMonth; day++)
            {
                var date = new DateTime(year, month, day);
                if (date.DayOfWeek != DayOfWeek.Saturday && date.DayOfWeek != DayOfWeek.Sunday)
                    days++;
            }
            return days;
        }

        private PayrollResponseDto MapToDto(Payroll payroll, Employee employee)
        {
            return new PayrollResponseDto
            {
                Id = payroll.Id,
                EmployeeName = employee.FullName,
                EmployeeCode = employee.EmployeeCode,
                Month = payroll.Month,
                Year = payroll.Year,
                BasicSalary = payroll.BasicSalary,
                TotalAllowance = payroll.TotalAllowance,
                TotalDeduction = payroll.TotalDeduction,
                OvertimePay = payroll.OvertimePay,
                NetSalary = payroll.NetSalary,
                WorkingDays = payroll.WorkingDays,
                PresentDays = payroll.PresentDays,
                LateDays = payroll.LateDays,
                AbsentDays = payroll.AbsentDays,
                OvertimeHours = payroll.OvertimeHours,
                Status = payroll.Status,
                Details = payroll.PayrollDetails.Select(d => new PayrollDetailDto
                {
                    Type = d.Type,
                    Description = d.Description,
                    Amount = d.Amount
                }).ToList()
            };
        }
    }
}