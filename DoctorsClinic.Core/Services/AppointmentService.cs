using DoctorsClinic.Core.Dtos;
using DoctorsClinic.Core.Dtos.Appointments;
using DoctorsClinic.Core.Extensions;
using DoctorsClinic.Core.Helper;
using DoctorsClinic.Core.IServices;
using DoctorsClinic.Domain.Enums;
using DoctorsClinic.Infrastructure.IRepositories;
using Mapster;
using Microsoft.EntityFrameworkCore;

namespace DoctorsClinic.Core.Services
{
    public class AppointmentService : IAppointmentService
    {
        private readonly IRepositoryWrapper _repo;

        public AppointmentService(IRepositoryWrapper repo)
        {
            _repo = repo;
        }

        // GetAllAsync
        public async Task<ResponseDto<PaginationDto<AppointmentDto>>> GetAllAsync(
            PaginationQuery pagination,
            AppointmentFilterDto filter,
            CancellationToken ct = default)
        {
            if (pagination == null)
                return new ResponseDto<PaginationDto<AppointmentDto>>(MsgResponce.Failed, true);

            var query = _repo.Appointments.GetAll(include: q =>
                q.Include(a => a.Doctor!)
                 .Include(a => a.Patient!),
                track: false
            );

            // Apply Filter
            if (filter != null)
            {
                if (filter.DoctorID.HasValue)
                    query = query.Where(a => a.DoctorID == filter.DoctorID.Value);
                if (filter.PatientID.HasValue)
                    query = query.Where(a => a.PatientID == filter.PatientID.Value);
                if (filter.Status != null)
                    query = query.Where(a => a.Status.ToString() == filter.Status);
                if (filter.DateFrom.HasValue)
                    query = query.Where(a => a.AppointmentDate >= filter.DateFrom.Value);
                if (filter.DateTo.HasValue)
                    query = query.Where(a => a.AppointmentDate <= filter.DateTo.Value);
            }

            var totalCount = await query.CountAsync(ct);
            var data = await query
                .ApplyPagging(pagination)
                .ProjectToType<AppointmentDto>()
                .ToListAsync(ct);

            var meta = new PaginationMetadata(totalCount, pagination);
            var paginated = new PaginationDto<AppointmentDto>(data, meta);

            if (!data.Any())
                return new ResponseDto<PaginationDto<AppointmentDto>>(MsgResponce.Appointment.NotFound, true);

            return new ResponseDto<PaginationDto<AppointmentDto>>(paginated);
        }

        // GetByDoctorAsync 
        public async Task<ResponseDto<List<AppointmentDto>>> GetByDoctorAsync(int doctorId, CancellationToken ct = default)
        {
            if (doctorId <= 0)
                return new ResponseDto<List<AppointmentDto>>(MsgResponce.Doctor.NotFound, true);

            var appointments = await _repo.Appointments
                .FindByCondition(a => a.DoctorID == doctorId, include: q =>
                    q.Include(a => a.Patient!), track: false)
                .ProjectToType<AppointmentDto>()
                .ToListAsync(ct);

            if (!appointments.Any())
                return new ResponseDto<List<AppointmentDto>>(MsgResponce.Appointment.NotFound, true);

            return new ResponseDto<List<AppointmentDto>>(appointments);
        }

        // GetByPatientAsync 
        public async Task<ResponseDto<List<AppointmentDto>>> GetByPatientAsync(int patientId, CancellationToken ct = default)
        {
            if (patientId <= 0)
                return new ResponseDto<List<AppointmentDto>>(MsgResponce.Patient.NotFound, true);

            var appointments = await _repo.Appointments
                .FindByCondition(a => a.PatientID == patientId, include: q =>
                    q.Include(a => a.Doctor!), track: false)
                .ProjectToType<AppointmentDto>()
                .ToListAsync(ct);

            if (!appointments.Any())
                return new ResponseDto<List<AppointmentDto>>(MsgResponce.Appointment.NotFound, true);

            return new ResponseDto<List<AppointmentDto>>(appointments);
        }

        // IsSlotAvailableAsync 
        public async Task<ResponseDto<bool>> IsSlotAvailableAsync(
            int doctorId,
            DateTime appointmentDate,
            int? excludeAppointmentId = null,
            CancellationToken ct = default)
        {
            if (doctorId <= 0)
                return new ResponseDto<bool>(MsgResponce.Doctor.NotFound, true);

            if (appointmentDate == default)
                return new ResponseDto<bool>("Appointment date is invalid.", true);

            var query = _repo.Appointments.FindByCondition(
                a => a.DoctorID == doctorId && a.AppointmentDate == appointmentDate,
                track: false);

            if (excludeAppointmentId.HasValue)
                query = query.Where(a => a.AppointmentID != excludeAppointmentId.Value);

            var exists = await query.AnyAsync(ct);
            return new ResponseDto<bool>(!exists);
        }

        // ChangeStatusAsync 
        public async Task<ResponseDto<AppointmentDto>> ChangeStatusAsync(
            int appointmentId,
            string status,
            CancellationToken ct = default)
        {
            if (appointmentId <= 0)
                return new ResponseDto<AppointmentDto>(MsgResponce.Appointment.NotFound, true);

            var appointment = await _repo.Appointments.GetByIdAsync(appointmentId, include: q =>
                q.Include(a => a.Doctor!).Include(a => a.Patient!), track: true);

            if (appointment == null)
                return new ResponseDto<AppointmentDto>(MsgResponce.Appointment.NotFound, true);

            if (!Enum.TryParse<Domain.Enums.AppointmentStatus>(status, out var statusEnum))
                return new ResponseDto<AppointmentDto>("Invalid appointment status.", true);

            appointment.Status = statusEnum;
            _repo.Appointments.Update(appointment);
            await _repo.SaveChangesAsync();

            var dto = appointment.Adapt<AppointmentDto>();
            return new ResponseDto<AppointmentDto>(dto);
        }

        // GetByIdAsync 
        public async Task<ResponseDto<AppointmentResponseDto>> GetByIdAsync(int id, CancellationToken ct = default)
        {
            if (id <= 0)
                return new ResponseDto<AppointmentResponseDto>(MsgResponce.Appointment.NotFound, true);

            var appointment = await _repo.Appointments.GetByIdAsync(id, include: q =>
                q.Include(a => a.Doctor!).Include(a => a.Patient!), track: false);

            if (appointment == null)
                return new ResponseDto<AppointmentResponseDto>(MsgResponce.Appointment.NotFound, true);

            var dto = appointment.Adapt<AppointmentResponseDto>();
            return new ResponseDto<AppointmentResponseDto>(dto);
        }

        // CreateAsync 
        public async Task<ResponseDto<AppointmentDto>> CreateAsync(CreateAppointmentDto dto, CancellationToken ct = default)
        {
            if (dto == null)
                return new ResponseDto<AppointmentDto>(MsgResponce.Failed, true);

            // Check Doctor 
            var doctor = await _repo.Doctors.GetByIdAsync(dto.DoctorID, track: false);
            if (doctor == null)
                return new ResponseDto<AppointmentDto>(MsgResponce.Doctor.NotFound, true);
            // Check Patient 
            var patient = await _repo.Patients.GetByIdAsync(dto.PatientID, track: false);
            if (patient == null)
                return new ResponseDto<AppointmentDto>(MsgResponce.Patient.NotFound, true);

            // Check Appointment
            var slotAvailableResp = await IsSlotAvailableAsync(dto.DoctorID, dto.AppointmentDate, null, ct);
            if (!slotAvailableResp.Data)
                return new ResponseDto<AppointmentDto>("Appointment slot is already booked.", true);

            // Create Appointment
            var appointment = dto.Adapt<Domain.Entities.Appointment>();
            appointment.Status = Domain.Enums.AppointmentStatus.Scheduled;
            await _repo.Appointments.AddAsync(appointment);
            await _repo.SaveChangesAsync();

            var resultDto = appointment.Adapt<AppointmentDto>();
            return new ResponseDto<AppointmentDto>(resultDto);
        }

        public async Task<ResponseDto<AppointmentDto>> UpdateAsync(int id, UpdateAppointmentDto dto, CancellationToken ct = default)
        {
            if (id <= 0 || dto == null)
                return new ResponseDto<AppointmentDto>(MsgResponce.Failed, true);

            var appointment = await _repo.Appointments.GetByIdAsync(id, track: true);
            if (appointment == null)
                return new ResponseDto<AppointmentDto>(MsgResponce.Appointment.NotFound, true);

            // Update
            appointment.AppointmentDate = dto.AppointmentDate ?? appointment.AppointmentDate;
            if(Enum.TryParse<AppointmentStatus>(dto.Status, out var status))
            appointment.Status = status;
            appointment.Notes = dto.Notes;

            if (dto.DoctorID.HasValue && dto.DoctorID.Value > 0)
            {
                var doctor = await _repo.Doctors.GetByIdAsync(dto.DoctorID.Value, track: false);
                if (doctor == null)
                    return new ResponseDto<AppointmentDto>(MsgResponce.Doctor.NotFound, true);

                appointment.DoctorID = dto.DoctorID.Value;
            }

            if (dto.PatientID.HasValue &&  dto.PatientID.Value > 0)
            {
                var patient = await _repo.Patients.GetByIdAsync(dto.PatientID.Value, track: false);
                if (patient == null)
                    return new ResponseDto<AppointmentDto>(MsgResponce.Patient.NotFound, true);

                appointment.PatientID = dto.PatientID.Value;
            }

            var slotAvailableResp = await IsSlotAvailableAsync(appointment.DoctorID, dto.AppointmentDate ?? appointment.AppointmentDate, id, ct);
            if (!slotAvailableResp.Data)
                return new ResponseDto<AppointmentDto>("Appointment slot is already booked.", true);

            _repo.Appointments.Update(appointment);
            await _repo.SaveChangesAsync();

            var resultDto = appointment.Adapt<AppointmentDto>();
            return new ResponseDto<AppointmentDto>(resultDto);
        }

        public async Task<ResponseDto<bool>> DeleteAsync(int id, CancellationToken ct = default)
        {
            if (id <= 0)
                return new ResponseDto<bool>(MsgResponce.Appointment.NotFound, true);

            var appointment = await _repo.Appointments.GetByIdAsync(id, track: true);
            if (appointment == null)
                return new ResponseDto<bool>(MsgResponce.Appointment.NotFound, true);

            _repo.Appointments.Remove(appointment);
            await _repo.SaveChangesAsync();

            return new ResponseDto<bool>(true);
        }
    }
}
