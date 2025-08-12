using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using DoctorsClinic.Core.Dtos.Prescriptions;
using DoctorsClinic.Core.Dtos;
using DoctorsClinic.Core.Helper;
using DoctorsClinic.Domain.Interfaces;

namespace DoctorsClinic.Core.IServices
{
    public interface IPrescriptionService : IScopedService
    {
        Task<ResponseDto<PaginationDto<PrescriptionDto>>> GetAllAsync(
            PaginationQuery pagination,
            PrescriptionFilterDto filter,
            CancellationToken ct = default);
        Task<ResponseDto<List<PrescriptionDto>>> GetByAppointmentAsync(int appointmentId, CancellationToken ct = default);
        Task<ResponseDto<PrescriptionResponseDto>> GetByIdAsync(int id, CancellationToken ct = default);
        Task<ResponseDto<PrescriptionDto>> CreateAsync(CreatePrescriptionDto dto, CancellationToken ct = default);
        Task<ResponseDto<PrescriptionDto>> UpdateAsync(int id, UpdatePrescriptionDto dto, CancellationToken ct = default);
        Task<ResponseDto<bool>> DeleteAsync(int id, CancellationToken ct = default);
    }
}
