using AbsensiApp.Application.DTOs;
using AbsensiApp.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AbsensiApp.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class DepartmentController : ControllerBase
    {
        private readonly IDepartmentService _departmentService;

        public DepartmentController(IDepartmentService departmentService)
        {
            _departmentService = departmentService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _departmentService.GetAllAsync();
            return Ok(new { success = true, data = result });
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var result = await _departmentService.GetByIdAsync(id);
            if (result == null) return NotFound(new { success = false, message = "Department tidak ditemukan" });
            return Ok(new { success = true, data = result });
        }

        [HttpPost]
        [Authorize(Roles = "Admin,HR")]
        public async Task<IActionResult> Create([FromBody] DepartmentRequestDto request)
        {
            var result = await _departmentService.CreateAsync(request);
            return Ok(new { success = true, data = result });
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin,HR")]
        public async Task<IActionResult> Update(Guid id, [FromBody] DepartmentRequestDto request)
        {
            var result = await _departmentService.UpdateAsync(id, request);
            if (result == null) return NotFound(new { success = false, message = "Department tidak ditemukan" });
            return Ok(new { success = true, data = result });
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var result = await _departmentService.DeleteAsync(id);
            if (!result) return NotFound(new { success = false, message = "Department tidak ditemukan" });
            return Ok(new { success = true, message = "Department berhasil dihapus" });
        }
    }
}