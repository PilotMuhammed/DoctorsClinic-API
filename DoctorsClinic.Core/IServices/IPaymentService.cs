using DoctorsClinic.Core.Dtos.Payments;
using DoctorsClinic.Core.Dtos;
using DoctorsClinic.Core.Helper;
using DoctorsClinic.Domain.Interfaces;

namespace DoctorsClinic.Core.IServices
{
    public interface IPaymentService : IScopedService
    {
        Task<ResponseDto<PaginationDto<PaymentDto>>> GetAll(PaginationQuery paginationQuery, PaymentFilterDto filter);
        Task<ResponseDto<IEnumerable<ListDto<int>>>> GetList();
        Task<ResponseDto<PaymentResponseDto>> GetById(int id);
        Task<ResponseDto<PaymentDto>> Add(CreatePaymentDto form);
        Task<ResponseDto<PaymentDto>> Update(int id, UpdatePaymentDto form);
        Task<ResponseDto<bool>> Delete(int id);
    }
}
