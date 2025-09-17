using DoctorsClinic.Core.Dtos.Patients;
using DoctorsClinic.Core.Dtos;
using DoctorsClinic.Core.Helper;
using DoctorsClinic.Domain.Interfaces;

namespace DoctorsClinic.Core.IServices
{
    public interface IPatientService : IScopedService
    {
        Task<ResponseDto<PaginationDto<PatientDto>>> GetAll(PaginationQuery paginationQuery, PatientFilterDto filter);
        Task<ResponseDto<IEnumerable<ListDto<int>>>> GetList();
        Task<ResponseDto<PatientResponseDto>> GetById(int id);
        Task<ResponseDto<PatientDto>> Add(CreatePatientDto form);
        Task<ResponseDto<PatientDto>> Update(int id, UpdatePatientDto form);
        Task<ResponseDto<bool>> Delete(int id);
    }
}
