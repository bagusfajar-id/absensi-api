namespace AbsensiApp.Application.DTOs
{
    public class EmployeeRequestDto
    {
        public string EmployeeCode { get; set; } = string.Empty;
        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;
        public string? PhoneNumber { get; set; }
        public string? Address { get; set; }
        public decimal BasicSalary { get; set; }
        public DateTime JoinDate { get; set; }
        public Guid DepartmentId { get; set; }
    }

    public class EmployeeUpdateDto
    {
        public string FullName { get; set; } = string.Empty;
        public string? PhoneNumber { get; set; }
        public string? Address { get; set; }
        public decimal BasicSalary { get; set; }
        public string Role { get; set; } = string.Empty;
        public Guid DepartmentId { get; set; }
        public bool IsActive { get; set; }
    }
}