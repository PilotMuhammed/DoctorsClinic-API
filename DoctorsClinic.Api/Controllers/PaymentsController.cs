using Api.Helper;
using DocotorClinic.Api.Controllers;
using DoctorsClinic.Core.Dtos.Payments;
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
    public class PaymentsController : BaseApiController
    {
        private readonly IPaymentService _paymentService;

        public PaymentsController(IPaymentService paymentService)
        {
            _paymentService = paymentService;
        }

        [AuthorizePermission(Permissions.Payments_View)]
        [HttpGet]
        public async Task<IActionResult> GetAll(
            [FromQuery] PaginationQuery pagination,
            [FromQuery] PaymentFilterDto filter,
            CancellationToken ct)
        {
            var result = await _paymentService.GetAllAsync(pagination, filter, ct);
            return Ok(result);
        }

        [AuthorizePermission(Permissions.Payments_View)]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id, CancellationToken ct)
        {
            var result = await _paymentService.GetByIdAsync(id, ct);
            if (result == null || result.Error)
                return NotFound(result);

            return Ok(result);
        }

        [AuthorizePermission(Permissions.Payments_Create)]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreatePaymentDto dto, CancellationToken ct)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _paymentService.CreateAsync(dto, ct);
            if (result.Error)
                return BadRequest(result);

            return Ok(result);
        }

        [AuthorizePermission(Permissions.Payments_Update)]
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdatePaymentDto dto, CancellationToken ct)
        {
            if (id != dto.PaymentID)
                return BadRequest("Payment ID mismatch.");

            var result = await _paymentService.UpdateAsync(id, dto, ct);
            if (result.Error)
                return BadRequest(result);

            return Ok(result);
        }

        [AuthorizePermission(Permissions.Payments_Delete)]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id, CancellationToken ct)
        {
            var result = await _paymentService.DeleteAsync(id, ct);
            if (result.Error)
                return BadRequest(result);

            return Ok(result);
        }
    }
}
