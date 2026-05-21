namespace AbsensiApp.Domain.Entities
{
    public class Employee
    {
        public Guid Id { get; set; }
        public string EmployeeCode { get; set; } = string.Empty;
        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;
        public string? PhoneNumber { get; set; }
        public string? Address { get; set; }
        public decimal BasicSalary { get; set; }
        public DateTime JoinDate { get; set; }
        public bool IsActive { get; set; } = true;
        public Guid DepartmentId { get; set; }
        public Department Department { get; set; } = null!;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}