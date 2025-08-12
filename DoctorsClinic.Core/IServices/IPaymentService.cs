using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using DoctorsClinic.Core.Dtos.Payments;
using DoctorsClinic.Core.Dtos;
using DoctorsClinic.Core.Helper;
using DoctorsClinic.Domain.Interfaces;

namespace DoctorsClinic.Core.IServices
{
    public interface IPaymentService : IScopedService
    {
        Task<ResponseDto<PaginationDto<PaymentDto>>> GetAllAsync(
            PaginationQuery pagination,
            PaymentFilterDto filter,
            CancellationToken ct = default);
        Task<ResponseDto<List<PaymentDto>>> GetByInvoiceAsync(int invoiceId, CancellationToken ct = default);
        Task<ResponseDto<PaymentResponseDto>> GetByIdAsync(int id, CancellationToken ct = default);
        Task<ResponseDto<PaymentDto>> CreateAsync(CreatePaymentDto dto, CancellationToken ct = default);
        Task<ResponseDto<PaymentDto>> UpdateAsync(int id, UpdatePaymentDto dto, CancellationToken ct = default);
        Task<ResponseDto<bool>> DeleteAsync(int id, CancellationToken ct = default);
    }
}
