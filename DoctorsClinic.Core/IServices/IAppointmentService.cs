using DoctorsClinic.Core.Dtos.Appointments;
using DoctorsClinic.Core.Dtos;
using DoctorsClinic.Core.Helper;
using DoctorsClinic.Domain.Interfaces;

namespace DoctorsClinic.Core.IServices
{
    public interface IAppointmentService : IScopedService
    {
        Task<ResponseDto<PaginationDto<AppointmentDto>>> GetAll(PaginationQuery paginationQuery, AppointmentFilterDto filter);
        /*
         Add inside GetAll -> GetByDoctor  &  GetByPatient
         */
        Task<ResponseDto<IEnumerable<ListDto<int>>>> GetList(); // Maybe Delete
        Task<ResponseDto<AppointmentResponseDto>> GetById(int id);  
        Task<ResponseDto<AppointmentDto>> Add(CreateAppointmentDto form);
        Task<ResponseDto<AppointmentDto>> Update(int id, UpdateAppointmentDto form);
        Task<ResponseDto<bool>> Delete(int id);
        Task<ResponseDto<bool>> IsAppointmentAvailable(int doctorId, DateTime appointmentDate);
    }
}
