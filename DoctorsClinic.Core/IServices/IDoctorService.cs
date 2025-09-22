using DoctorsClinic.Core.Dtos.Doctors;
using DoctorsClinic.Core.Dtos;
using DoctorsClinic.Core.Helper;
using DoctorsClinic.Domain.Interfaces;

namespace DoctorsClinic.Core.IServices
{
    public interface IDoctorService : IScopedService
    {
        Task<ResponseDto<PaginationDto<DoctorDto>>> GetAll(PaginationQuery paginationQuery, DoctorFilterDto filter);
        Task<ResponseDto<IEnumerable<ListDto<int>>>> GetList();
        Task<ResponseDto<DoctorResponseDto>> GetById(int id);
        Task<ResponseDto<DoctorDto>> Add(CreateDoctorDto form);
        Task<ResponseDto<DoctorDto>> Update(int id, UpdateDoctorDto form);
        Task<ResponseDto<bool>> Delete(int id);
    }
}
