using Api.Helper;
using DocotorClinic.Api.Controllers;
using DoctorsClinic.Core.Dtos.Invoices;
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
    public class InvoicesController : BaseApiController
    {
        private readonly IInvoiceService _invoiceService;

        public InvoicesController(IInvoiceService invoiceService)
        {
            _invoiceService = invoiceService;
        }

        [AuthorizePermission(Permissions.Invoices_View)]
        [HttpGet]
        public async Task<IActionResult> GetAll(
            [FromQuery] PaginationQuery pagination,
            [FromQuery] InvoiceFilterDto filter,
            CancellationToken ct)
        {
            var result = await _invoiceService.GetAllAsync(pagination, filter, ct);
            return Ok(result);
        }

        [AuthorizePermission(Permissions.Invoices_View)]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id, CancellationToken ct)
        {
            var result = await _invoiceService.GetByIdAsync(id, ct);
            if (result == null || result.Error)
                return NotFound(result);

            return Ok(result);
        }

        [AuthorizePermission(Permissions.Invoices_Create)]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateInvoiceDto dto, CancellationToken ct)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _invoiceService.CreateAsync(dto, ct);
            if (result.Error)
                return BadRequest(result);

            return Ok(result);
        }

        [AuthorizePermission(Permissions.Invoices_Update)]
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateInvoiceDto dto, CancellationToken ct)
        {
            if (id != dto.InvoiceID)
                return BadRequest("Invoice ID mismatch.");

            var result = await _invoiceService.UpdateAsync(id, dto, ct);
            if (result.Error)
                return BadRequest(result);

            return Ok(result);
        }

        [AuthorizePermission(Permissions.Invoices_Delete)]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id, CancellationToken ct)
        {
            var result = await _invoiceService.DeleteAsync(id, ct);
            if (result.Error)
                return BadRequest(result);

            return Ok(result);
        }
    }
}
