using DoctorsClinic.Core.Dtos;
using DoctorsClinic.Core.Dtos.Appointments;
using DoctorsClinic.Core.Extensions;
using DoctorsClinic.Core.Helper;
using DoctorsClinic.Core.IServices;
using DoctorsClinic.Core.IServices.Account;
using DoctorsClinic.Domain.Entities;
using DoctorsClinic.Domain.Enums;
using DoctorsClinic.Infrastructure.IRepositories;
using Mapster;
using Microsoft.EntityFrameworkCore;

namespace DoctorsClinic.Core.Services
{
    public class AppointmentService : IAppointmentService
    {
        private readonly IRepositoryWrapper _wrapper;
        private readonly IUserAccessorService _userAccessor;
        public AppointmentService(IRepositoryWrapper wrapper, IUserAccessorService userAccessor)
        {
            _wrapper = wrapper;
            _userAccessor = userAccessor;
        }

        public async Task<ResponseDto<PaginationDto<AppointmentDto>>> GetAll(PaginationQuery paginationQuery, AppointmentFilterDto filter)
        {
            #region Apply Filter
            var query = _wrapper.AppointmentRepo.GetAll()
                .Include(a => a.Doctor)
                .Include(a => a.Patient)
                .Where(a => !filter.PatientID.HasValue || a.PatientID == filter.PatientID)
                .Where(a => !filter.DoctorID.HasValue || a.DoctorID == filter.DoctorID)
                .Where(a => !filter.AppointmentDate.HasValue || a.AppointmentDate == filter.AppointmentDate)
                .Where(a => !filter.Status.HasValue || a.Status == filter.Status)
                .Where(a => string.IsNullOrEmpty(filter.Notes) || a.Notes!.ToLower().Contains(filter.Notes.ToLower()))
                .Where(a => !filter.CreatedAt.HasValue || a.CreatedAt == filter.CreatedAt);
            #endregion

            var data = await query
                .OrderByDescending(a => a.CreatedAt)
                .ApplyPagging(paginationQuery)
                .ProjectToType<AppointmentDto>()
                .ToListAsync();

            var count = await query.CountAsync();
            var metadata = new PaginationMetadata(count, paginationQuery);

            return new ResponseDto<PaginationDto<AppointmentDto>>(
                new PaginationDto<AppointmentDto>(data, metadata));
        }

        public async Task<ResponseDto<AppointmentResponseDto>> GetById(int id)
        {
            var appointment = await _wrapper.AppointmentRepo.FindByCondition(a => a.Id == id)
                .Include(a => a.Doctor)
                .Include(a => a.Patient)
                .Include(a => a.Prescriptions)
                .Include(a => a.Invoice)
                .FirstOrDefaultAsync();

            if (appointment == null)
                return new ResponseDto<AppointmentResponseDto>(MsgResponce.Appointment.NotFound, true);

            return new ResponseDto<AppointmentResponseDto>(appointment.Adapt<AppointmentResponseDto>());
        }

        public async Task<ResponseDto<AppointmentDto>> Add(CreateAppointmentDto form)
        {
            var patient = await _wrapper.PatientRepo.FindItemByCondition(p => p.Id == form.PatientID);
            if (patient == null)
                return new ResponseDto<AppointmentDto>(MsgResponce.Patient.NotFound, true);

            var doctor = await _wrapper.DoctorRepo.FindItemByCondition(d => d.Id == form.DoctorID);
            if (doctor == null)
                return new ResponseDto<AppointmentDto>(MsgResponce.Doctor.NotFound, true);

            var availableAppointment = await IsAppointmentAvailable(form.DoctorID, form.AppointmentDate);
            if (availableAppointment.Error)
                return new ResponseDto<AppointmentDto>(MsgResponce.Appointment.AlreadyBooked, true);

            var appointment = form.Adapt<Appointment>();
            appointment.CreatorId = _userAccessor.UserId;
            appointment.CreatedAt = DateTime.Now;

            await _wrapper.AppointmentRepo.Insert(appointment);
            await _wrapper.SaveAllAsync();

            appointment = await _wrapper.AppointmentRepo.FindByCondition(a => a.Id == appointment.Id)
                .Include(a => a.Patient)
                .Include(a => a.Doctor)
                .FirstOrDefaultAsync();

            return new ResponseDto<AppointmentDto>(appointment.Adapt<AppointmentDto>());
        }

        public async Task<ResponseDto<AppointmentDto>> Update(int id, UpdateAppointmentDto form)
        {
            var appointment = await _wrapper.AppointmentRepo.FindByCondition(a => a.Id == id)
                .Include(a => a.Patient)
                .Include(a => a.Doctor)
                .FirstOrDefaultAsync();
            if (appointment == null)
                return new ResponseDto<AppointmentDto>(MsgResponce.Appointment.NotFound, true);

            var patient = await _wrapper.PatientRepo.FindItemByCondition(p => p.Id == form.PatientID);
            if (patient == null)
                return new ResponseDto<AppointmentDto>(MsgResponce.Patient.NotFound, true);

            var doctor = await _wrapper.DoctorRepo.FindItemByCondition(d => d.Id == form.DoctorID);
            if (doctor == null)
                return new ResponseDto<AppointmentDto>(MsgResponce.Doctor.NotFound, true);

            var availableAppointment = await IsAppointmentAvailable(form.DoctorID, form.AppointmentDate, id);
            if (availableAppointment.Error)
                return new ResponseDto<AppointmentDto>(MsgResponce.Appointment.AlreadyBooked, true);

            var saveAppointment = form.Adapt(appointment);
            saveAppointment.ModifierId = _userAccessor.UserId;
            saveAppointment.ModifieAt = DateTime.Now;

            await _wrapper.AppointmentRepo.Update(saveAppointment);
            return new ResponseDto<AppointmentDto>(saveAppointment.Adapt<AppointmentDto>());
        }

        public async Task<ResponseDto<bool>> Delete(int id)
        {
            var appointment = await _wrapper.AppointmentRepo.FindItemByCondition(a => a.Id == id);
            if (appointment == null)
                return new ResponseDto<bool>(MsgResponce.Appointment.NotFound, true);

            appointment.DeleterId = _userAccessor.UserId;
            await _wrapper.AppointmentRepo.Delete(id);
            await _wrapper.SaveAllAsync();
            return new ResponseDto<bool>(true);
        }

        public async Task<ResponseDto<bool>> IsAppointmentAvailable(int? doctorId, DateTime? appointmentDate, int? excludeAppointmentId = null)
        {
            var query = _wrapper.AppointmentRepo.GetAll()
                .Where(a => a.DoctorID == doctorId && a.AppointmentDate == appointmentDate && a.Status != AppointmentStatus.Cancelled);

            if (excludeAppointmentId.HasValue)
                query = query.Where(a => a.Id != excludeAppointmentId.Value);

            if (!await query.AnyAsync())
                return new ResponseDto<bool>(MsgResponce.Appointment.CurrentlyAvailable, true);
            else
                return new ResponseDto<bool>(MsgResponce.Appointment.AlreadyBooked, true);
        }
    }
}