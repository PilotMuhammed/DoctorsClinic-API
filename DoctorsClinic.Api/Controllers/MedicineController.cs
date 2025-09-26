using Api.Helper;
using DocotorClinic.Api.Controllers;
using DoctorsClinic.Core.Dtos;
using DoctorsClinic.Core.Dtos.Medicines;
using DoctorsClinic.Core.Helper;
using DoctorsClinic.Core.IServices;
using DoctorsClinic.Domain.Enums;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Api.Controllers.Medicines
{
    public class MedicinesController : BaseApiController
    {
        private readonly IMedicineService _service;
        public MedicinesController(IMedicineService service)
        {
            _service = service;
        }

        [AuthorizePermission(EPermission.Medicines_View)]
        [Produces("application/json")]
        [ProducesResponseType(typeof(ResponseDto<PaginationDto<MedicineDto>>), (int)HttpStatusCode.OK)]
        [HttpGet]
        public async Task<ActionResult> GetAll([FromQuery] PaginationQuery paginationQuery, [FromQuery] MedicineFilterDto filter)
        {
            var response = await _service.GetAll(paginationQuery, filter);
            return Ok(response);
        }

        [AuthorizePermission(EPermission.Medicines_View)]
        [Produces("application/json")]
        [ProducesResponseType(typeof(ResponseDto<IEnumerable<ListDto<int>>>), (int)HttpStatusCode.OK)]
        [HttpGet("[action]")]
        public async Task<ActionResult> GetList()
        {
            var response = await _service.GetList();
            return Ok(response);
        }

        [AuthorizePermission(EPermission.Medicines_View)]
        [Produces("application/json")]
        [ProducesResponseType(typeof(ResponseDto<MedicineResponseDto>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ResponseDto<string>), (int)HttpStatusCode.BadRequest)]
        [HttpGet("{id}")]
        public async Task<ActionResult> GetById(int id)
        {
            var response = await _service.GetById(id);
            return response.Error
                ? BadRequest(response)
                : Ok(response);
        }

        [AuthorizePermission(EPermission.Medicines_Create)]
        [Produces("application/json")]
        [ProducesResponseType(typeof(ResponseDto<MedicineDto>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ResponseDto<string>), (int)HttpStatusCode.BadRequest)]
        [HttpPost]
        public async Task<ActionResult> Post([FromBody] CreateMedicineDto form)
        {
            var response = await _service.Add(form);
            return response.Error
                ? BadRequest(response)
                : Ok(response);
        }

        [AuthorizePermission(EPermission.Medicines_Update)]
        [Produces("application/json")]
        [ProducesResponseType(typeof(ResponseDto<MedicineDto>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ResponseDto<string>), (int)HttpStatusCode.BadRequest)]
        [HttpPut("{id}")]
        public async Task<ActionResult> Put(int id, [FromBody] UpdateMedicineDto form)
        {
            var response = await _service.Update(id, form);
            return response.Error
                ? BadRequest(response)
                : Ok(response);
        }

        [AuthorizePermission(EPermission.Medicines_Delete)]
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
