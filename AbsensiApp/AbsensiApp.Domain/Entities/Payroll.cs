namespace AbsensiApp.Domain.Entities
{
    public class Payroll
    {
        public Guid Id { get; set; }
        public Guid EmployeeId { get; set; }
        public Employee Employee { get; set; } = null!;
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
        public string Status { get; set; } = "Draft"; // Draft, Approved, Paid
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public ICollection<PayrollDetail> PayrollDetails { get; set; } = new List<PayrollDetail>();
    }
}