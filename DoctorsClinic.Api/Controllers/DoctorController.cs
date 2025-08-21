using Api.Helper;
using DocotorClinic.Api.Controllers;
using DoctorsClinic.Core.Dtos.Doctors;
using DoctorsClinic.Core.Dtos.Permission;
using DoctorsClinic.Core.Helper;
using DoctorsClinic.Core.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class DoctorsController : BaseApiController
    {
        private readonly IDoctorService _doctorService;

        public DoctorsController(IDoctorService doctorService)
        {
            _doctorService = doctorService;
        }

        [AuthorizePermission(Permissions.Doctors_View)]
        [HttpGet]
        public async Task<IActionResult> GetAll(
            [FromQuery] PaginationQuery pagination,
            [FromQuery] DoctorFilterDto filter,
            CancellationToken ct)
        {
            var result = await _doctorService.GetAllAsync(pagination, filter, ct);
            return Ok(result);
        }

        [AuthorizePermission(Permissions.Doctors_View)]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id, CancellationToken ct)
        {
            var result = await _doctorService.GetByIdAsync(id, ct);
            if (result == null || result.Error)
                return NotFound(result);
            return Ok(result);
        }

        [AuthorizePermission(Permissions.Doctors_Create)]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateDoctorDto dto, CancellationToken ct)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _doctorService.CreateAsync(dto, ct);
            if (result.Error)
                return BadRequest(result);

            return Ok(result);
        }

        [AuthorizePermission(Permissions.Doctors_Update)]
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateDoctorDto dto, CancellationToken ct)
        {
            if (id != dto.DoctorID)
                return BadRequest("Doctor ID mismatch.");

            var result = await _doctorService.UpdateAsync(id, dto, ct);
            if (result.Error)
                return BadRequest(result);

            return Ok(result);
        }

        [AuthorizePermission(Permissions.Doctors_Delete)]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id, CancellationToken ct)
        {
            var result = await _doctorService.DeleteAsync(id, ct);
            if (result.Error)
                return BadRequest(result);

            return Ok(result);
        }
    }
}
