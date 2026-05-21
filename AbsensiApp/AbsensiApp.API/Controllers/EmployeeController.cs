using AbsensiApp.Application.DTOs;
using AbsensiApp.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AbsensiApp.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class EmployeeController : ControllerBase
    {
        private readonly IEmployeeService _employeeService;

        public EmployeeController(IEmployeeService employeeService)
        {
            _employeeService = employeeService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _employeeService.GetAllAsync();
            return Ok(new { success = true, data = result });
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var result = await _employeeService.GetByIdAsync(id);
            if (result == null) return NotFound(new { success = false, message = "Employee tidak ditemukan" });
            return Ok(new { success = true, data = result });
        }

        [HttpPost]
        [Authorize(Roles = "Admin,HR")]
        public async Task<IActionResult> Create([FromBody] EmployeeRequestDto request)
        {
            var result = await _employeeService.CreateAsync(request);
            return Ok(new { success = true, data = result });
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin,HR")]
        public async Task<IActionResult> Update(Guid id, [FromBody] EmployeeUpdateDto request)
        {
            var result = await _employeeService.UpdateAsync(id, request);
            if (result == null) return NotFound(new { success = false, message = "Employee tidak ditemukan" });
            return Ok(new { success = true, data = result });
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var result = await _employeeService.DeleteAsync(id);
            if (!result) return NotFound(new { success = false, message = "Employee tidak ditemukan" });
            return Ok(new { success = true, message = "Employee berhasil dihapus" });
        }
    }
}