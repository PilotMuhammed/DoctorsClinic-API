using DoctorsClinic.Core.Dtos;
using DoctorsClinic.Core.Dtos.Payments;
using DoctorsClinic.Core.Extensions;
using DoctorsClinic.Core.Helper;
using DoctorsClinic.Core.IServices;
using DoctorsClinic.Infrastructure.IRepositories;
using Mapster;
using Microsoft.EntityFrameworkCore;

namespace DoctorsClinic.Core.Services
{
    public class PaymentService : IPaymentService
    {
        private readonly IRepositoryWrapper _repo;

        public PaymentService(IRepositoryWrapper repo)
        {
            _repo = repo;
        }

        // GetAllAsync 
        public async Task<ResponseDto<PaginationDto<PaymentDto>>> GetAllAsync(
            PaginationQuery pagination,
            PaymentFilterDto filter,
            CancellationToken ct = default)
        {
            if (pagination == null)
                return new ResponseDto<PaginationDto<PaymentDto>>(MsgResponce.Failed, true);

            var query = _repo.Payments.GetAll(track: false);

            if (filter != null)
            {
                if (filter.InvoiceID.HasValue)
                    query = query.Where(p => p.InvoiceID == filter.InvoiceID.Value);

                if (!string.IsNullOrWhiteSpace(filter.PaymentMethod))
                    if(Enum.TryParse<Domain.Enums.PaymentMethod>(filter.PaymentMethod, true, out var paymentMethodEnum))
                    query = query.Where(p => p.PaymentMethod == paymentMethodEnum);

                if (filter.DateFrom.HasValue)
                    query = query.Where(p => p.Date >= filter.DateFrom.Value);
                if (filter.DateTo.HasValue)
                    query = query.Where(p => p.Date <= filter.DateTo.Value);
                if (filter.AmountFrom.HasValue)
                    query = query.Where(p => p.Amount >= filter.AmountFrom.Value);
                if (filter.AmountTo.HasValue)
                    query = query.Where(p => p.Amount <= filter.AmountTo.Value);
            }

            var totalCount = await query.CountAsync(ct);
            var data = await query
                .ApplyPagging(pagination)
                .ProjectToType<PaymentDto>()
                .ToListAsync(ct);

            var meta = new PaginationMetadata(totalCount, pagination);
            var paginated = new PaginationDto<PaymentDto>(data, meta);

            if (!data.Any())
                return new ResponseDto<PaginationDto<PaymentDto>>(MsgResponce.Payment.Failed, true);

            return new ResponseDto<PaginationDto<PaymentDto>>(paginated);
        }

        // GetByInvoiceAsync
        public async Task<ResponseDto<List<PaymentDto>>> GetByInvoiceAsync(int invoiceId, CancellationToken ct = default)
        {
            if (invoiceId <= 0)
                return new ResponseDto<List<PaymentDto>>(MsgResponce.Invoice.NotFound, true);

            var payments = await _repo.Payments
                .FindByCondition(p => p.InvoiceID == invoiceId, track: false)
                .ProjectToType<PaymentDto>()
                .ToListAsync(ct);

            if (!payments.Any())
                return new ResponseDto<List<PaymentDto>>(MsgResponce.Payment.Failed, true);

            return new ResponseDto<List<PaymentDto>>(payments);
        }

        // GetByIdAsync
        public async Task<ResponseDto<PaymentResponseDto>> GetByIdAsync(int id, CancellationToken ct = default)
        {
            if (id <= 0)
                return new ResponseDto<PaymentResponseDto>(MsgResponce.Payment.Failed, true);

            var payment = await _repo.Payments.GetWithInvoiceAsync(id);
            if (payment == null)
                return new ResponseDto<PaymentResponseDto>(MsgResponce.Payment.Failed, true);

            var paymentDto = payment.Adapt<PaymentDto>();
            var invoiceDto = payment.Invoice?.Adapt<Core.Dtos.Invoices.InvoiceDto>();

            var response = new PaymentResponseDto
            {
                Payment = paymentDto,
                Invoice = invoiceDto
            };

            return new ResponseDto<PaymentResponseDto>(response);
        }

        // CreateAsync
        public async Task<ResponseDto<PaymentDto>> CreateAsync(CreatePaymentDto dto, CancellationToken ct = default)
        {
            if (dto == null)
                return new ResponseDto<PaymentDto>(MsgResponce.Failed, true);

            // Check Invoice
            var invoice = await _repo.Invoices.GetByIdAsync(dto.InvoiceID, track: false);
            if (invoice == null)
                return new ResponseDto<PaymentDto>(MsgResponce.Invoice.NotFound, true);

            if (dto.Amount <= 0)
                return new ResponseDto<PaymentDto>("Amount must be greater than zero.", true);

            if (string.IsNullOrWhiteSpace(dto.PaymentMethod))
                return new ResponseDto<PaymentDto>("Payment method is required.", true);

            var payment = dto.Adapt<Domain.Entities.Payment>();
            await _repo.Payments.AddAsync(payment);
            await _repo.SaveChangesAsync();

            var resultDto = payment.Adapt<PaymentDto>();
            return new ResponseDto<PaymentDto>(resultDto);
        }

        // UpdateAsync
        public async Task<ResponseDto<PaymentDto>> UpdateAsync(int id, UpdatePaymentDto dto, CancellationToken ct = default)
        {
            if (id <= 0 || dto == null)
                return new ResponseDto<PaymentDto>(MsgResponce.Failed, true);

            var payment = await _repo.Payments.GetByIdAsync(id, track: true);
            if (payment == null)
                return new ResponseDto<PaymentDto>(MsgResponce.Payment.Failed, true);

            if (dto.InvoiceID.HasValue && dto.InvoiceID.Value > 0 && dto.InvoiceID != payment.InvoiceID)
            {
                var invoice = await _repo.Invoices.GetByIdAsync(dto.InvoiceID.Value, track: false);
                if (invoice == null)
                    return new ResponseDto<PaymentDto>(MsgResponce.Invoice.NotFound, true);

                payment.InvoiceID = dto.InvoiceID.Value;
            }

            if (dto.Amount.HasValue && dto.Amount.Value > 0)
                payment.Amount = dto.Amount.Value;
            if (dto.Date.HasValue)
                payment.Date = dto.Date.Value;

            if (!string.IsNullOrWhiteSpace(dto.PaymentMethod))
                if (Enum.TryParse<Domain.Enums.PaymentMethod>(dto.PaymentMethod, true, out var paymentMethodEnum))
                    payment.PaymentMethod = paymentMethodEnum;

            _repo.Payments.Update(payment);
            await _repo.SaveChangesAsync();

            var resultDto = payment.Adapt<PaymentDto>();
            return new ResponseDto<PaymentDto>(resultDto);
        }

        // DeleteAsync
        public async Task<ResponseDto<bool>> DeleteAsync(int id, CancellationToken ct = default)
        {
            if (id <= 0)
                return new ResponseDto<bool>(MsgResponce.Payment.Failed, true);

            var payment = await _repo.Payments.GetByIdAsync(id, track: true);
            if (payment == null)
                return new ResponseDto<bool>(MsgResponce.Payment.Failed, true);

            _repo.Payments.Remove(payment);
            await _repo.SaveChangesAsync();

            return new ResponseDto<bool>(true);
        }
    }
}
