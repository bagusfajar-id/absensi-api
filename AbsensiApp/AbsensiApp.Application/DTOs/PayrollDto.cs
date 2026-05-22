namespace AbsensiApp.Application.DTOs
{
    public class GeneratePayrollRequestDto
    {
        public Guid EmployeeId { get; set; }
        public int Month { get; set; }
        public int Year { get; set; }
        public int OvertimeHours { get; set; }
    }

    public class PayrollResponseDto
    {
        public Guid Id { get; set; }
        public string EmployeeName { get; set; } = string.Empty;
        public string EmployeeCode { get; set; } = string.Empty;
        public int Month { get; set; }
        public int Year { get; set; }
        public decimal BasicSalary { get; set; }
        public decimal TotalAllowance { get; set; }
        public decimal TotalDeduction { get; set; }
        public decimal OvertimePay { get; set; }
        public decimal NetSalary { get; set; }
        public int WorkingDays { get; set; }
        public int PresentDays { get; set; }
        public int LateDays { get; set; }
        public int AbsentDays { get; set; }
        public int OvertimeHours { get; set; }
        public string Status { get; set; } = string.Empty;
        public List<PayrollDetailDto> Details { get; set; } = new();
    }

    public class PayrollDetailDto
    {
        public string Type { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public decimal Amount { get; set; }
    }
}