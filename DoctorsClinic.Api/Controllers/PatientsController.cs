using Api.Helper;
using DocotorClinic.Api.Controllers;
using DoctorsClinic.Core.Dtos.Patients;
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
    public class PatientsController : BaseApiController
    {
        private readonly IPatientService _patientService;

        public PatientsController(IPatientService patientService)
        {
            _patientService = patientService;
        }

        [AuthorizePermission(Permissions.Patients_View)]
        [HttpGet]
        public async Task<IActionResult> GetAll(
            [FromQuery] PaginationQuery pagination,
            [FromQuery] PatientFilterDto filter,
            CancellationToken ct)
        {
            var result = await _patientService.GetAllAsync(pagination, filter, ct);
            return Ok(result);
        }

        [AuthorizePermission(Permissions.Patients_View)]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id, CancellationToken ct)
        {
            var result = await _patientService.GetByIdAsync(id, ct);
            if (result == null || result.Error)
                return NotFound(result);

            return Ok(result);
        }

        [AuthorizePermission(Permissions.Patients_Create)]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreatePatientDto dto, CancellationToken ct)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _patientService.CreateAsync(dto, ct);
            if (result.Error)
                return BadRequest(result);

            return Ok(result);
        }

        [AuthorizePermission(Permissions.Patients_Update)]
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdatePatientDto dto, CancellationToken ct)
        {
            if (id != dto.PatientID)
                return BadRequest("Patient ID mismatch.");

            var result = await _patientService.UpdateAsync(id, dto, ct);
            if (result.Error)
                return BadRequest(result);

            return Ok(result);
        }

        [AuthorizePermission(Permissions.Patients_Delete)]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id, CancellationToken ct)
        {
            var result = await _patientService.DeleteAsync(id, ct);
            if (result.Error)
                return BadRequest(result);

            return Ok(result);
        }
    }
}
