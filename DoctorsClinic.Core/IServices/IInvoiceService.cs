using DoctorsClinic.Core.Dtos.Invoices;
using DoctorsClinic.Core.Dtos;
using DoctorsClinic.Core.Helper;
using DoctorsClinic.Domain.Interfaces;

namespace DoctorsClinic.Core.IServices
{
    public interface IInvoiceService : IScopedService
    {
        Task<ResponseDto<PaginationDto<InvoiceDto>>> GetAll(PaginationQuery paginationQuery, InvoiceFilterDto filter);
        Task<ResponseDto<IEnumerable<ListDto<int>>>> GetList();
        Task<ResponseDto<InvoiceResponseDto>> GetById(int id);
        Task<ResponseDto<InvoiceDto>> Add(CreateInvoiceDto form);
        Task<ResponseDto<InvoiceDto>> Update(int id, UpdateInvoiceDto form);
        Task<ResponseDto<bool>> Delete(int id);
    }
}
