using AbsensiApp.Application.DTOs;
using AbsensiApp.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace AbsensiApp.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class PayrollController : ControllerBase
    {
        private readonly IPayrollService _payrollService;

        public PayrollController(IPayrollService payrollService)
        {
            _payrollService = payrollService;
        }

        [HttpPost("generate")]
        [Authorize(Roles = "Admin,HR")]
        public async Task<IActionResult> Generate([FromBody] GeneratePayrollRequestDto request)
        {
            try
            {
                var result = await _payrollService.GeneratePayrollAsync(request);
                return Ok(new { success = true, data = result });
            }
            catch (Exception ex)
            {
                return BadRequest(new { success = false, message = ex.Message });
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var result = await _payrollService.GetByIdAsync(id);
            if (result == null) return NotFound(new { success = false, message = "Payroll tidak ditemukan" });
            return Ok(new { success = true, data = result });
        }

        [HttpGet]
        [Authorize(Roles = "Admin,HR")]
        public async Task<IActionResult> GetAllByPeriod([FromQuery] int month, [FromQuery] int year)
        {
            var result = await _payrollService.GetAllByPeriodAsync(month, year);
            return Ok(new { success = true, data = result });
        }

        [HttpPut("{id}/approve")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Approve(Guid id)
        {
            var result = await _payrollService.ApprovePayrollAsync(id);
            if (result == null) return NotFound(new { success = false, message = "Payroll tidak ditemukan" });
            return Ok(new { success = true, data = result });
        }

        [HttpGet("my-payroll")]
        public async Task<IActionResult> GetMyPayroll([FromQuery] int month, [FromQuery] int year)
        {
            try
            {
                var employeeId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
                var result = await _payrollService.GetMyPayrollAsync(employeeId, month, year);
                if (result == null) return NotFound(new { success = false, message = "Payroll tidak ditemukan" });
                return Ok(new { success = true, data = result });
            }
            catch (Exception ex)
            {
                return BadRequest(new { success = false, message = ex.Message });
            }
        }
    }
}