using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using DoctorsClinic.Core.Dtos.Patients;
using DoctorsClinic.Core.Dtos;
using DoctorsClinic.Core.Helper;
using DoctorsClinic.Domain.Interfaces;

namespace DoctorsClinic.Core.IServices
{
    public interface IPatientService : IScopedService
    {
        Task<ResponseDto<PaginationDto<PatientDto>>> GetAllAsync(
            PaginationQuery pagination,
            PatientFilterDto filter,
            CancellationToken ct = default);
        Task<ResponseDto<IEnumerable<ListDto<int>>>> GetListAsync(CancellationToken ct = default);
        Task<ResponseDto<PatientResponseDto>> GetByIdAsync(int id, CancellationToken ct = default);
        Task<ResponseDto<PatientDto>> CreateAsync(CreatePatientDto dto, CancellationToken ct = default);
        Task<ResponseDto<PatientDto>> UpdateAsync(int id, UpdatePatientDto dto, CancellationToken ct = default);
        Task<ResponseDto<bool>> DeleteAsync(int id, CancellationToken ct = default);
    }
}
