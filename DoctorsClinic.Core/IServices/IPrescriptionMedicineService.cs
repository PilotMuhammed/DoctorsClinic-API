using DoctorsClinic.Core.Dtos.PrescriptionMedicines;
using DoctorsClinic.Core.Dtos;
using DoctorsClinic.Core.Helper;
using DoctorsClinic.Domain.Interfaces;

namespace DoctorsClinic.Core.IServices
{
    public interface IPrescriptionMedicineService : IScopedService
    {
        Task<ResponseDto<PaginationDto<PrescriptionMedicineDto>>> GetAll(PaginationQuery paginationQuery, PrescriptionMedicineFilterDto filter);
        Task<ResponseDto<PrescriptionMedicineResponseDto>> GetById(int id);
        Task<ResponseDto<PrescriptionMedicineDto>> Add(CreatePrescriptionMedicineDto form);
        Task<ResponseDto<PrescriptionMedicineDto>> Update(int id, UpdatePrescriptionMedicineDto form);
        Task<ResponseDto<bool>> Delete(int id);
    }
}
