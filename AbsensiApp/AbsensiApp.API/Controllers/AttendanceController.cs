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
    public class AttendanceController : ControllerBase
    {
        private readonly IAttendanceService _attendanceService;

        public AttendanceController(IAttendanceService attendanceService)
        {
            _attendanceService = attendanceService;
        }

        [HttpPost("generate-qr")]
        [Authorize(Roles = "Admin,HR")]
        public async Task<IActionResult> GenerateQr()
        {
            try
            {
                var result = await _attendanceService.GenerateQrCodeAsync();
                return Ok(new { success = true, data = result });
            }
            catch (Exception ex)
            {
                return BadRequest(new { success = false, message = ex.Message });
            }
        }

        [HttpPost("checkin-qr")]
        public async Task<IActionResult> CheckInQr([FromBody] CheckInQrRequestDto request)
        {
            try
            {
                var employeeId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
                var result = await _attendanceService.CheckInQrAsync(employeeId, request);
                return Ok(new { success = true, data = result });
            }
            catch (Exception ex)
            {
                return BadRequest(new { success = false, message = ex.Message });
            }
        }

        [HttpPost("checkin-gps")]
        public async Task<IActionResult> CheckInGps([FromBody] CheckInGpsRequestDto request)
        {
            try
            {
                var employeeId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
                var result = await _attendanceService.CheckInGpsAsync(employeeId, request);
                return Ok(new { success = true, data = result });
            }
            catch (Exception ex)
            {
                return BadRequest(new { success = false, message = ex.Message });
            }
        }

        [HttpPost("checkout")]
        public async Task<IActionResult> CheckOut([FromBody] CheckOutRequestDto request)
        {
            try
            {
                var employeeId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
                var result = await _attendanceService.CheckOutAsync(employeeId, request);
                return Ok(new { success = true, data = result });
            }
            catch (Exception ex)
            {
                return BadRequest(new { success = false, message = ex.Message });
            }
        }

        [HttpGet("my-attendance")]
        public async Task<IActionResult> GetMyAttendance()
        {
            try
            {
                var employeeId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
                var result = await _attendanceService.GetMyAttendanceAsync(employeeId);
                return Ok(new { success = true, data = result });
            }
            catch (Exception ex)
            {
                return BadRequest(new { success = false, message = ex.Message });
            }
        }

        [HttpGet]
        [Authorize(Roles = "Admin,HR")]
        public async Task<IActionResult> GetAll([FromQuery] DateTime? date)
        {
            try
            {
                var result = await _attendanceService.GetAllAttendanceAsync(date);
                return Ok(new { success = true, data = result });
            }
            catch (Exception ex)
            {
                return BadRequest(new { success = false, message = ex.Message });
            }
        }
    }
}