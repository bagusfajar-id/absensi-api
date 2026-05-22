namespace AbsensiApp.Domain.Entities
{
    public class PayrollDetail
    {
        public Guid Id { get; set; }
        public Guid PayrollId { get; set; }
        public Payroll Payroll { get; set; } = null!;
        public string Type { get; set; } = string.Empty; // Allowance, Deduction, Overtime
        public string Description { get; set; } = string.Empty;
        public decimal Amount { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}