using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using DoctorsClinic.Core.Dtos.Appointments;
using DoctorsClinic.Core.Dtos;
using DoctorsClinic.Core.Helper;
using DoctorsClinic.Domain.Interfaces;

namespace DoctorsClinic.Core.IServices
{
    public interface IAppointmentService : IScopedService
    {
        Task<ResponseDto<PaginationDto<AppointmentDto>>> GetAllAsync(
            PaginationQuery pagination,
            AppointmentFilterDto filter,
            CancellationToken ct = default);
        Task<ResponseDto<List<AppointmentDto>>> GetByDoctorAsync(int doctorId, CancellationToken ct = default);
        Task<ResponseDto<List<AppointmentDto>>> GetByPatientAsync(int patientId, CancellationToken ct = default);
        Task<ResponseDto<bool>> IsSlotAvailableAsync(
            int doctorId,
            DateTime appointmentDate,
            int? excludeAppointmentId = null,
            CancellationToken ct = default);
        Task<ResponseDto<AppointmentDto>> ChangeStatusAsync(
            int appointmentId,
            string status,
            CancellationToken ct = default);
        Task<ResponseDto<AppointmentResponseDto>> GetByIdAsync(int id, CancellationToken ct = default);
        Task<ResponseDto<AppointmentDto>> CreateAsync(CreateAppointmentDto dto, CancellationToken ct = default);
        Task<ResponseDto<AppointmentDto>> UpdateAsync(int id, UpdateAppointmentDto dto, CancellationToken ct = default);
        Task<ResponseDto<bool>> DeleteAsync(int id, CancellationToken ct = default);
    }
}
