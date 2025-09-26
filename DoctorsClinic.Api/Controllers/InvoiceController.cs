using Api.Helper;
using DocotorClinic.Api.Controllers;
using DoctorsClinic.Core.Dtos;
using DoctorsClinic.Core.Dtos.Invoices;
using DoctorsClinic.Core.Helper;
using DoctorsClinic.Core.IServices;
using DoctorsClinic.Domain.Enums;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Api.Controllers
{
    public class InvoicesController : BaseApiController
    {
        private readonly IInvoiceService _service;
        public InvoicesController(IInvoiceService service)
        {
            _service = service;
        }

        [AuthorizePermission(EPermission.Invoices_View)]
        [Produces("application/json")]
        [ProducesResponseType(typeof(ResponseDto<PaginationDto<InvoiceDto>>), (int)HttpStatusCode.OK)]
        [HttpGet]
        public async Task<ActionResult> GetAll([FromQuery] PaginationQuery paginationQuery, [FromQuery] InvoiceFilterDto filter)
        {
            var response = await _service.GetAll(paginationQuery, filter);
            return Ok(response);
        }

        [AuthorizePermission(EPermission.Invoices_View)]
        [Produces("application/json")]
        [ProducesResponseType(typeof(ResponseDto<InvoiceResponseDto>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ResponseDto<string>), (int)HttpStatusCode.BadRequest)]
        [HttpGet("{id}")]
        public async Task<ActionResult> GetById(int id)
        {
            var response = await _service.GetById(id);
            return response.Error
                ? BadRequest(response)
                : Ok(response);
        }

        [AuthorizePermission(EPermission.Invoices_Create)]
        [Produces("application/json")]
        [ProducesResponseType(typeof(ResponseDto<InvoiceDto>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ResponseDto<string>), (int)HttpStatusCode.BadRequest)]
        [HttpPost]
        public async Task<ActionResult> Post([FromBody] CreateInvoiceDto form)
        {
            var response = await _service.Add(form);
            return response.Error
                ? BadRequest(response)
                : Ok(response);
        }

        [AuthorizePermission(EPermission.Invoices_Update)]
        [Produces("application/json")]
        [ProducesResponseType(typeof(ResponseDto<InvoiceDto>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ResponseDto<string>), (int)HttpStatusCode.BadRequest)]
        [HttpPut("{id}")]
        public async Task<ActionResult> Put(int id, [FromBody] UpdateInvoiceDto form)
        {
            var response = await _service.Update(id, form);
            return response.Error
                ? BadRequest(response)
                : Ok(response);
        }

        [AuthorizePermission(EPermission.Invoices_Delete)]
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
