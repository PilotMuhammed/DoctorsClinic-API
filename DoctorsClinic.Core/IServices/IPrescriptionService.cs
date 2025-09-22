using DoctorsClinic.Core.Dtos.Prescriptions;
using DoctorsClinic.Core.Dtos;
using DoctorsClinic.Core.Helper;
using DoctorsClinic.Domain.Interfaces;

namespace DoctorsClinic.Core.IServices
{
    public interface IPrescriptionService : IScopedService
    {
        Task<ResponseDto<PaginationDto<PrescriptionDto>>> GetAll(PaginationQuery paginationQuery, PrescriptionFilterDto filter);
        Task<ResponseDto<PrescriptionResponseDto>> GetById(int id);
        Task<ResponseDto<PrescriptionDto>> Add(CreatePrescriptionDto form);
        Task<ResponseDto<PrescriptionDto>> Update(int id, UpdatePrescriptionDto form);
        Task<ResponseDto<bool>> Delete(int id);
    }
}
