using Api.Helper;
using DocotorClinic.Api.Controllers;
using DoctorsClinic.Core.Dtos.Medicines;
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
    public class MedicinesController : BaseApiController
    {
        private readonly IMedicineService _medicineService;

        public MedicinesController(IMedicineService medicineService)
        {
            _medicineService = medicineService;
        }

        [AuthorizePermission(Permissions.Medicines_View)]
        [HttpGet]
        public async Task<IActionResult> GetAll(
            [FromQuery] PaginationQuery pagination,
            [FromQuery] MedicineFilterDto filter,
            CancellationToken ct)
        {
            var result = await _medicineService.GetAllAsync(pagination, filter, ct);
            return Ok(result);
        }

        [AuthorizePermission(Permissions.Medicines_View)]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id, CancellationToken ct)
        {
            var result = await _medicineService.GetByIdAsync(id, ct);
            if (result == null || result.Error)
                return NotFound(result);

            return Ok(result);
        }

        [AuthorizePermission(Permissions.Medicines_Create)]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateMedicineDto dto, CancellationToken ct)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _medicineService.CreateAsync(dto, ct);
            if (result.Error)
                return BadRequest(result);

            return Ok(result);
        }

        [AuthorizePermission(Permissions.Medicines_Update)]
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateMedicineDto dto, CancellationToken ct)
        {
            if (id != dto.MedicineID)
                return BadRequest("Medicine ID mismatch.");

            var result = await _medicineService.UpdateAsync(id, dto, ct);
            if (result.Error)
                return BadRequest(result);

            return Ok(result);
        }

        [AuthorizePermission(Permissions.Medicines_Delete)]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id, CancellationToken ct)
        {
            var result = await _medicineService.DeleteAsync(id, ct);
            if (result.Error)
                return BadRequest(result);

            return Ok(result);
        }
    }
}
