using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using DoctorsClinic.Core.Dtos.Invoices;
using DoctorsClinic.Core.Dtos;
using DoctorsClinic.Core.Helper;
using DoctorsClinic.Domain.Interfaces;

namespace DoctorsClinic.Core.IServices
{
    public interface IInvoiceService : IScopedService
    {
        Task<ResponseDto<PaginationDto<InvoiceDto>>> GetAllAsync(
            PaginationQuery pagination,
            InvoiceFilterDto filter,
            CancellationToken ct = default);
        Task<ResponseDto<InvoiceResponseDto>> GetByIdAsync(int id, CancellationToken ct = default);
        Task<ResponseDto<List<InvoiceDto>>> GetByPatientAsync(int patientId, CancellationToken ct = default);
        Task<ResponseDto<InvoiceDto>> CreateAsync(CreateInvoiceDto dto, CancellationToken ct = default);
        Task<ResponseDto<InvoiceDto>> GenerateForAppointmentAsync(int appointmentId, CancellationToken ct = default);
        Task<ResponseDto<InvoiceDto>> UpdateAsync(int id, UpdateInvoiceDto dto, CancellationToken ct = default);
        Task<ResponseDto<InvoiceDto>> MarkAsPaidAsync(int invoiceId, CancellationToken ct = default);
        Task<ResponseDto<bool>> DeleteAsync(int id, CancellationToken ct = default);
    }
}
