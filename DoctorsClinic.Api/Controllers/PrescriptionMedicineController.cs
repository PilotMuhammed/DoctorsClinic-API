using Api.Helper;
using DocotorClinic.Api.Controllers;
using DoctorsClinic.Core.Dtos.Permission;
using DoctorsClinic.Core.Dtos.PrescriptionMedicines;
using DoctorsClinic.Core.Helper;
using DoctorsClinic.Core.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class PrescriptionMedicineController : BaseApiController
    {
        private readonly IPrescriptionMedicineService _service;

        public PrescriptionMedicineController(IPrescriptionMedicineService service)
        {
            _service = service;
        }

        [AuthorizePermission(Permissions.Prescriptions_View, Permissions.Medicines_View, Permissions.MedicalRecords_View)]
        [HttpGet]
        public async Task<IActionResult> GetAll(
            [FromQuery] PaginationQuery pagination,
            [FromQuery] PrescriptionMedicineFilterDto filter,
            CancellationToken ct)
        {
            var result = await _service.GetAllAsync(pagination, filter, ct);
            return Ok(result);
        }

        [AuthorizePermission(Permissions.Prescriptions_View, Permissions.Medicines_View, Permissions.MedicalRecords_View)]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id, CancellationToken ct)
        {
            var result = await _service.GetByIdAsync(id, ct);
            if (result == null || result.Error)
                return NotFound(result);

            return Ok(result);
        }

        [AuthorizePermission(Permissions.Prescriptions_Create, Permissions.Medicines_Create, Permissions.MedicalRecords_Create)]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreatePrescriptionMedicineDto dto, CancellationToken ct)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _service.CreateAsync(dto, ct);
            if (result.Error)
                return BadRequest(result);

            return Ok(result);
        }

        [AuthorizePermission(Permissions.Prescriptions_Update, Permissions.Medicines_Update, Permissions.MedicalRecords_Update)]
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdatePrescriptionMedicineDto dto, CancellationToken ct)
        {
            if (id != dto.ID)
                return BadRequest("PrescriptionMedicine ID mismatch.");

            var result = await _service.UpdateAsync(id, dto, ct);
            if (result.Error)
                return BadRequest(result);

            return Ok(result);
        }

        [AuthorizePermission(Permissions.Prescriptions_Delete, Permissions.Medicines_Delete, Permissions.MedicalRecords_Delete)]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id, CancellationToken ct)
        {
            var result = await _service.DeleteAsync(id, ct);
            if (result.Error)
                return BadRequest(result);

            return Ok(result);
        }
    }
}
