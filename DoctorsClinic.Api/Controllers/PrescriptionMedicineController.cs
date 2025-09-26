using Api.Helper;
using DocotorClinic.Api.Controllers;
using DoctorsClinic.Core.Dtos;
using DoctorsClinic.Core.Dtos.PrescriptionMedicines;
using DoctorsClinic.Core.Helper;
using DoctorsClinic.Core.IServices;
using DoctorsClinic.Domain.Enums;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Api.Controllers
{
    public class PrescriptionMedicineController : BaseApiController
    {
        private readonly IPrescriptionMedicineService _service;
        public PrescriptionMedicineController(IPrescriptionMedicineService service)
        {
            _service = service;
        }

        [AuthorizePermission(EPermission.Prescriptions_View, EPermission.Medicines_View)]
        [Produces("application/json")]
        [ProducesResponseType(typeof(ResponseDto<PaginationDto<PrescriptionMedicineDto>>), (int)HttpStatusCode.OK)]
        [HttpGet]
        public async Task<ActionResult> GetAll([FromQuery] PaginationQuery paginationQuery, [FromQuery] PrescriptionMedicineFilterDto filter)
        {
            var response = await _service.GetAll(paginationQuery, filter);
            return Ok(response);
        }

        [AuthorizePermission(EPermission.Prescriptions_View, EPermission.Medicines_View)]
        [Produces("application/json")]
        [ProducesResponseType(typeof(ResponseDto<PrescriptionMedicineResponseDto>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ResponseDto<string>), (int)HttpStatusCode.BadRequest)]
        [HttpGet("{id}")]
        public async Task<ActionResult> GetById(int id)
        {
            var response = await _service.GetById(id);
            return response.Error
                ? BadRequest(response)
                : Ok(response);
        }

        [AuthorizePermission(EPermission.Prescriptions_Create, EPermission.Medicines_Create)]
        [Produces("application/json")]
        [ProducesResponseType(typeof(ResponseDto<PrescriptionMedicineDto>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ResponseDto<string>), (int)HttpStatusCode.BadRequest)]
        [HttpPost]
        public async Task<ActionResult> Post([FromBody] CreatePrescriptionMedicineDto form)
        {
            var response = await _service.Add(form);
            return response.Error
                ? BadRequest(response)
                : Ok(response);
        }

        [AuthorizePermission(EPermission.Prescriptions_Update, EPermission.Medicines_Update)]
        [Produces("application/json")]
        [ProducesResponseType(typeof(ResponseDto<PrescriptionMedicineDto>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ResponseDto<string>), (int)HttpStatusCode.BadRequest)]
        [HttpPut("{id}")]
        public async Task<ActionResult> Put(int id, [FromBody] UpdatePrescriptionMedicineDto form)
        {
            var response = await _service.Update(id, form);
            return response.Error
                ? BadRequest(response)
                : Ok(response);
        }

        [AuthorizePermission(EPermission.Prescriptions_Delete, EPermission.Medicines_Delete)]
        [Produces("application/json")]
        [ProducesResponseType(typeof(ResponseDto<bool>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ResponseDto<string>), (int)HttpStatusCode.BadRequest)]
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var response = await _service.Delete(id);
            return response.Error
                ? BadRequest(response)
                : Ok(response);
        }
    }
}
