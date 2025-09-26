using Api.Helper;
using DocotorClinic.Api.Controllers;
using DoctorsClinic.Core.Dtos;
using DoctorsClinic.Core.Dtos.Patients;
using DoctorsClinic.Core.Helper;
using DoctorsClinic.Core.IServices;
using DoctorsClinic.Domain.Enums;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Api.Controllers.Patients
{
    public class PatientsController : BaseApiController
    {
        private readonly IPatientService _service;
        public PatientsController(IPatientService service)
        {
            _service = service;
        }

        [AuthorizePermission(EPermission.Patients_View)]
        [Produces("application/json")]
        [ProducesResponseType(typeof(ResponseDto<PaginationDto<PatientDto>>), (int)HttpStatusCode.OK)]
        [HttpGet]
        public async Task<ActionResult> GetAll([FromQuery] PaginationQuery paginationQuery, [FromQuery] PatientFilterDto filter)
        {
            var response = await _service.GetAll(paginationQuery, filter);
            return Ok(response);
        }

        [AuthorizePermission(EPermission.Patients_View)]
        [Produces("application/json")]
        [ProducesResponseType(typeof(ResponseDto<IEnumerable<ListDto<int>>>), (int)HttpStatusCode.OK)]
        [HttpGet("[action]")]
        public async Task<ActionResult> GetList()
        {
            var response = await _service.GetList();
            return Ok(response);
        }

        [AuthorizePermission(EPermission.Patients_View)]
        [Produces("application/json")]
        [ProducesResponseType(typeof(ResponseDto<PatientResponseDto>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ResponseDto<string>), (int)HttpStatusCode.BadRequest)]
        [HttpGet("{id}")]
        public async Task<ActionResult> GetById(int id)
        {
            var response = await _service.GetById(id);
            return response.Error
                ? BadRequest(response)
                : Ok(response);
        }

        [AuthorizePermission(EPermission.Patients_Create)]
        [Produces("application/json")]
        [ProducesResponseType(typeof(ResponseDto<PatientDto>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ResponseDto<string>), (int)HttpStatusCode.BadRequest)]
        [HttpPost]
        public async Task<ActionResult> Post([FromBody] CreatePatientDto form)
        {
            var response = await _service.Add(form);
            return response.Error
                ? BadRequest(response)
                : Ok(response);
        }

        [AuthorizePermission(EPermission.Patients_Update)]
        [Produces("application/json")]
        [ProducesResponseType(typeof(ResponseDto<PatientDto>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ResponseDto<string>), (int)HttpStatusCode.BadRequest)]
        [HttpPut("{id}")]
        public async Task<ActionResult> Put(int id, [FromBody] UpdatePatientDto form)
        {
            var response = await _service.Update(id, form);
            return response.Error
                ? BadRequest(response)
                : Ok(response);
        }

        [AuthorizePermission(EPermission.Patients_Delete)]
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
