using Api.Helper;
using DocotorClinic.Api.Controllers;
using DoctorsClinic.Core.Dtos;
using DoctorsClinic.Core.Dtos.Appointments;
using DoctorsClinic.Core.Helper;
using DoctorsClinic.Core.IServices;
using DoctorsClinic.Domain.Enums;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Api.Controllers
{
    public class AppointmentsController : BaseApiController
    {
        private readonly IAppointmentService _service;

        public AppointmentsController(IAppointmentService service)
        {
            _service = service;
        }

        [AuthorizePermission(EPermission.Appointments_View)]
        [Produces("application/json")]
        [ProducesResponseType(typeof(ResponseDto<PaginationDto<AppointmentDto>>), (int)HttpStatusCode.OK)]
        [HttpGet]
        public async Task<ActionResult> GetAll([FromQuery] PaginationQuery paginationQuery, [FromQuery] AppointmentFilterDto filter)
        {
            var response = await _service.GetAll(paginationQuery, filter);
            return Ok(response);
        }

        [AuthorizePermission(EPermission.Appointments_View)]
        [Produces("application/json")]
        [ProducesResponseType(typeof(ResponseDto<AppointmentResponseDto>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ResponseDto<string>), (int)HttpStatusCode.BadRequest)]
        [HttpGet("{id}")]
        public async Task<ActionResult> GetById(int id)
        {
            var response = await _service.GetById(id);
            return response.Error
                ? BadRequest(response)
                : Ok(response);
        }

        [AuthorizePermission(EPermission.Appointments_Create)]
        [Produces("application/json")]
        [ProducesResponseType(typeof(ResponseDto<AppointmentDto>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ResponseDto<string>), (int)HttpStatusCode.BadRequest)]
        [HttpPost]
        public async Task<ActionResult> Post([FromBody] CreateAppointmentDto form)
        {
            var response = await _service.Add(form);
            return response.Error
                ? BadRequest(response)
                : Ok(response);
        }

        [AuthorizePermission(EPermission.Appointments_Update)]
        [Produces("application/json")]
        [ProducesResponseType(typeof(ResponseDto<AppointmentDto>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ResponseDto<string>), (int)HttpStatusCode.BadRequest)]
        [HttpPut("{id}")]
        public async Task<ActionResult> Put(int id, [FromBody] UpdateAppointmentDto form)
        {
            var response = await _service.Update(id, form);
            return response.Error
                ? BadRequest(response)
                : Ok(response);
        }

        [AuthorizePermission(EPermission.Appointments_Delete)]
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

        [AuthorizePermission(EPermission.Appointments_View, EPermission.Appointments_Create, EPermission.Appointments_Update)]
        [Produces("application/json")]
        [ProducesResponseType(typeof(ResponseDto<bool>), (int)HttpStatusCode.OK)]
        [HttpGet("IsAvailable")]
        public async Task<ActionResult> IsAppointmentAvailable([FromQuery] int? doctorId, [FromQuery] DateTime? appointmentDate, [FromQuery] int? excludeAppointmentId = null)
        {
            var response = await _service.IsAppointmentAvailable(doctorId, appointmentDate, excludeAppointmentId);
            return Ok(response);
        }
    }
}
