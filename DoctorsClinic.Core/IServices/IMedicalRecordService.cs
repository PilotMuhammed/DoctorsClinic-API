using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using DoctorsClinic.Core.Dtos.MedicalRecords;
using DoctorsClinic.Core.Dtos;
using DoctorsClinic.Core.Helper;
using DoctorsClinic.Domain.Interfaces;

namespace DoctorsClinic.Core.IServices
{
    public interface IMedicalRecordService : IScopedService
    {
        Task<ResponseDto<PaginationDto<MedicalRecordDto>>> GetAllAsync(
            PaginationQuery pagination,
            MedicalRecordFilterDto filter,
            CancellationToken ct = default);
        Task<ResponseDto<List<MedicalRecordDto>>> GetByPatientAsync(int patientId, CancellationToken ct = default);
        Task<ResponseDto<MedicalRecordResponseDto>> GetByIdAsync(int id, CancellationToken ct = default);
        Task<ResponseDto<MedicalRecordDto>> CreateAsync(CreateMedicalRecordDto dto, CancellationToken ct = default);
        Task<ResponseDto<MedicalRecordDto>> UpdateAsync(int id, UpdateMedicalRecordDto dto, CancellationToken ct = default);
        Task<ResponseDto<bool>> DeleteAsync(int id, CancellationToken ct = default);
    }
}
