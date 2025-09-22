using DoctorsClinic.Core.Dtos.MedicalRecords;
using DoctorsClinic.Core.Dtos;
using DoctorsClinic.Core.Helper;
using DoctorsClinic.Domain.Interfaces;

namespace DoctorsClinic.Core.IServices
{
    public interface IMedicalRecordService : IScopedService
    {
        Task<ResponseDto<PaginationDto<MedicalRecordDto>>> GetAll(PaginationQuery paginationQuery, MedicalRecordFilterDto filter);
        Task<ResponseDto<MedicalRecordResponseDto>> GetById(int id);
        Task<ResponseDto<MedicalRecordDto>> Add(CreateMedicalRecordDto form);
        Task<ResponseDto<MedicalRecordDto>> Update(int id, UpdateMedicalRecordDto form);
        Task<ResponseDto<bool>> Delete(int id);
    }
}
