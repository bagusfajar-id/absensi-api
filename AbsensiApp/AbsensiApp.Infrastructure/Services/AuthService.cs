using AbsensiApp.Application.DTOs;
using AbsensiApp.Application.Interfaces;
using AbsensiApp.Domain.Entities;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace AbsensiApp.Infrastructure.Services
{
    public class AuthService : IAuthService
    {
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IConfiguration _configuration;

        public AuthService(IEmployeeRepository employeeRepository, IConfiguration configuration)
        {
            _employeeRepository = employeeRepository;
            _configuration = configuration;
        }

        public async Task<LoginResponseDto> LoginAsync(LoginRequestDto request)
        {
            var employee = await _employeeRepository.GetByEmailAsync(request.Email);

            if (employee == null || !BCrypt.Net.BCrypt.Verify(request.Password, employee.PasswordHash))
                throw new UnauthorizedAccessException("Email atau password salah!");

            if (!employee.IsActive)
                throw new UnauthorizedAccessException("Akun tidak aktif!");

            var token = GenerateJwtToken(employee);

            return new LoginResponseDto
            {
                Token = token,
                ExpiredAt = DateTime.UtcNow.AddMinutes(
                    double.Parse(_configuration["JwtSettings:ExpiryInMinutes"]!)),
                Employee = new EmployeeDto
                {
                    Id = employee.Id,
                    EmployeeCode = employee.EmployeeCode,
                    FullName = employee.FullName,
                    Email = employee.Email,
                    Role = employee.Role,
                    PhoneNumber = employee.PhoneNumber,
                    Address = employee.Address,
                    BasicSalary = employee.BasicSalary,
                    JoinDate = employee.JoinDate,
                    IsActive = employee.IsActive,
                    DepartmentName = employee.Department?.Name ?? ""
                }
            };
        }

        private string GenerateJwtToken(Employee employee)
        {
            var jwtSettings = _configuration.GetSection("JwtSettings");
            var secretKey = jwtSettings["SecretKey"];
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey!));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, employee.Id.ToString()),
                new Claim(ClaimTypes.Email, employee.Email),
                new Claim(ClaimTypes.Name, employee.FullName),
                new Claim(ClaimTypes.Role, employee.Role)
            };

            var token = new JwtSecurityToken(
                issuer: jwtSettings["Issuer"],
                audience: jwtSettings["Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(
                    double.Parse(jwtSettings["ExpiryInMinutes"]!)),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}