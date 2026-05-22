using AbsensiApp.Application.DTOs;
using AbsensiApp.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AbsensiApp.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class OfficeLocationController : ControllerBase
    {
        private readonly IOfficeLocationService _officeLocationService;

        public OfficeLocationController(IOfficeLocationService officeLocationService)
        {
            _officeLocationService = officeLocationService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _officeLocationService.GetAllAsync();
            return Ok(new { success = true, data = result });
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create([FromBody] OfficeLocationRequestDto request)
        {
            var result = await _officeLocationService.CreateAsync(request);
            return Ok(new { success = true, data = result });
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Update(Guid id, [FromBody] OfficeLocationRequestDto request)
        {
            var result = await _officeLocationService.UpdateAsync(id, request);
            if (result == null) return NotFound(new { success = false, message = "Lokasi tidak ditemukan" });
            return Ok(new { success = true, data = result });
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var result = await _officeLocationService.DeleteAsync(id);
            if (!result) return NotFound(new { success = false, message = "Lokasi tidak ditemukan" });
            return Ok(new { success = true, message = "Lokasi berhasil dihapus" });
        }
    }
}