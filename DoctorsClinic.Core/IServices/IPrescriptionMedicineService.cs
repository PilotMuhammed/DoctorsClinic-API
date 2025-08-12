using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using DoctorsClinic.Core.Dtos.PrescriptionMedicines;
using DoctorsClinic.Core.Dtos;
using DoctorsClinic.Core.Helper;
using DoctorsClinic.Domain.Interfaces;

namespace DoctorsClinic.Core.IServices
{
    public interface IPrescriptionMedicineService : IScopedService
    {
        Task<ResponseDto<PaginationDto<PrescriptionMedicineDto>>> GetAllAsync(
            PaginationQuery pagination,
            PrescriptionMedicineFilterDto filter,
            CancellationToken ct = default);
        Task<ResponseDto<List<PrescriptionMedicineDto>>> GetByPrescriptionAsync(
            int prescriptionId,
            CancellationToken ct = default);
        Task<ResponseDto<PrescriptionMedicineResponseDto>> GetByIdAsync(int id, CancellationToken ct = default);
        Task<ResponseDto<PrescriptionMedicineDto>> CreateAsync(CreatePrescriptionMedicineDto dto, CancellationToken ct = default);
        Task<ResponseDto<PrescriptionMedicineDto>> UpdateAsync(int id, UpdatePrescriptionMedicineDto dto, CancellationToken ct = default);
        Task<ResponseDto<bool>> DeleteAsync(int id, CancellationToken ct = default);
    }
}
