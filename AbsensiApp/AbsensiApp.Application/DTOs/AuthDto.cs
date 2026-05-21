namespace AbsensiApp.Application.DTOs
{
    public class LoginRequestDto
    {
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }

    public class LoginResponseDto
    {
        public string Token { get; set; } = string.Empty;
        public DateTime ExpiredAt { get; set; }
        public EmployeeDto Employee { get; set; } = null!;
    }

    public class EmployeeDto
    {
        public Guid Id { get; set; }
        public string EmployeeCode { get; set; } = string.Empty;
        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;
        public string? PhoneNumber { get; set; }
        public string? Address { get; set; }
        public decimal BasicSalary { get; set; }
        public DateTime JoinDate { get; set; }
        public bool IsActive { get; set; }
        public string DepartmentName { get; set; } = string.Empty;
    }
}