using DoctorsClinic.Core.Dtos;
using DoctorsClinic.Core.Dtos.Prescriptions;
using DoctorsClinic.Core.Extensions;
using DoctorsClinic.Core.Helper;
using DoctorsClinic.Core.IServices;
using DoctorsClinic.Infrastructure.IRepositories;
using Mapster;
using Microsoft.EntityFrameworkCore;

namespace DoctorsClinic.Core.Services
{
    public class PrescriptionService : IPrescriptionService
    {
        private readonly IRepositoryWrapper _repo;

        public PrescriptionService(IRepositoryWrapper repo)
        {
            _repo = repo;
        }

        // GetAllAsync
        public async Task<ResponseDto<PaginationDto<PrescriptionDto>>> GetAllAsync(
            PaginationQuery pagination,
            PrescriptionFilterDto filter,
            CancellationToken ct = default)
        {
            if (pagination == null)
                return new ResponseDto<PaginationDto<PrescriptionDto>>(MsgResponce.Failed, true);

            var query = _repo.Prescriptions.GetAll(track: false);

            if (filter != null)
            {
                if (filter.DoctorID.HasValue)
                    query = query.Where(p => p.DoctorID == filter.DoctorID.Value);
                if (filter.PatientID.HasValue)
                    query = query.Where(p => p.PatientID == filter.PatientID.Value);
                if (filter.DateFrom.HasValue)
                    query = query.Where(p => p.Date >= filter.DateFrom.Value);
                if (filter.DateTo.HasValue)
                    query = query.Where(p => p.Date <= filter.DateTo.Value);
            }

            var totalCount = await query.CountAsync(ct);
            var data = await query
                .ApplyPagging(pagination)
                .ProjectToType<PrescriptionDto>()
                .ToListAsync(ct);

            var meta = new PaginationMetadata(totalCount, pagination);
            var paginated = new PaginationDto<PrescriptionDto>(data, meta);

            if (!data.Any())
                return new ResponseDto<PaginationDto<PrescriptionDto>>(MsgResponce.Failed, true);

            return new ResponseDto<PaginationDto<PrescriptionDto>>(paginated);
        }

        // GetByAppointmentAsync 
        public async Task<ResponseDto<List<PrescriptionDto>>> GetByAppointmentAsync(int appointmentId, CancellationToken ct = default)
        {
            if (appointmentId <= 0)
                return new ResponseDto<List<PrescriptionDto>>(MsgResponce.Failed, true);

            var prescriptions = await _repo.Prescriptions
                .FindByCondition(p => p.AppointmentID == appointmentId, track: false)
                .ProjectToType<PrescriptionDto>()
                .ToListAsync(ct);

            if (!prescriptions.Any())
                return new ResponseDto<List<PrescriptionDto>>(MsgResponce.Failed, true);

            return new ResponseDto<List<PrescriptionDto>>(prescriptions);
        }

        // GetByIdAsync
        public async Task<ResponseDto<PrescriptionResponseDto>> GetByIdAsync(int id, CancellationToken ct = default)
        {
            if (id <= 0)
                return new ResponseDto<PrescriptionResponseDto>(MsgResponce.Failed, true);

            var prescription = await _repo.Prescriptions.GetWithAllDetailsAsync(id);
            if (prescription == null)
                return new ResponseDto<PrescriptionResponseDto>(MsgResponce.Failed, true);

            var dto = prescription.Adapt<PrescriptionDto>();
            var appointmentDto = prescription.Appointment?.Adapt<Core.Dtos.Appointments.AppointmentDto>();
            var doctorDto = prescription.Doctor?.Adapt<Core.Dtos.Doctors.DoctorDto>();
            var patientDto = prescription.Patient?.Adapt<Core.Dtos.Patients.PatientDto>();
            var prescriptionMedicines = prescription.PrescriptionMedicines?
                .Select(pm => pm.Adapt<Core.Dtos.PrescriptionMedicines.PrescriptionMedicineDto>()).ToList()
                ?? new List<Core.Dtos.PrescriptionMedicines.PrescriptionMedicineDto>();

            var response = new PrescriptionResponseDto
            {
                Prescription = dto,
                Appointment = appointmentDto,
                Doctor = doctorDto,
                Patient = patientDto,
                PrescriptionMedicines = prescriptionMedicines
            };

            return new ResponseDto<PrescriptionResponseDto>(response);
        }

        // CreateAsync
        public async Task<ResponseDto<PrescriptionDto>> CreateAsync(CreatePrescriptionDto dto, CancellationToken ct = default)
        {
            if (dto == null)
                return new ResponseDto<PrescriptionDto>(MsgResponce.Failed, true);

            var appointment = await _repo.Appointments.GetByIdAsync(dto.AppointmentID, track: false);
            if (appointment == null)
                return new ResponseDto<PrescriptionDto>("Appointment not found.", true);

            var doctor = await _repo.Doctors.GetByIdAsync(dto.DoctorID, track: false);
            if (doctor == null)
                return new ResponseDto<PrescriptionDto>("Doctor not found.", true);

            var patient = await _repo.Patients.GetByIdAsync(dto.PatientID, track: false);
            if (patient == null)
                return new ResponseDto<PrescriptionDto>("Patient not found.", true);

            var prescription = dto.Adapt<Domain.Entities.Prescription>();
            await _repo.Prescriptions.AddAsync(prescription);
            await _repo.SaveChangesAsync();

            var resultDto = prescription.Adapt<PrescriptionDto>();
            return new ResponseDto<PrescriptionDto>(resultDto);
        }

        // UpdateAsync 
        public async Task<ResponseDto<PrescriptionDto>> UpdateAsync(int id, UpdatePrescriptionDto dto, CancellationToken ct = default)
        {
            if (id <= 0 || dto == null)
                return new ResponseDto<PrescriptionDto>(MsgResponce.Failed, true);

            var prescription = await _repo.Prescriptions.GetByIdAsync(id, track: true);
            if (prescription == null)
                return new ResponseDto<PrescriptionDto>(MsgResponce.Failed, true);

            if (dto.AppointmentID.HasValue && dto.AppointmentID.Value > 0 && dto.AppointmentID != prescription.AppointmentID)
            {
                var appointment = await _repo.Appointments.GetByIdAsync(dto.AppointmentID.Value, track: false);
                if (appointment == null)
                    return new ResponseDto<PrescriptionDto>("Appointment not found.", true);

                prescription.AppointmentID = dto.AppointmentID.Value;
            }

            if (dto.DoctorID.HasValue && dto.DoctorID.Value > 0 && dto.DoctorID != prescription.DoctorID)
            {
                var doctor = await _repo.Doctors.GetByIdAsync(dto.DoctorID.Value, track: false);
                if (doctor == null)
                    return new ResponseDto<PrescriptionDto>("Doctor not found.", true);

                prescription.DoctorID = dto.DoctorID.Value;
            }

            if (dto.PatientID.HasValue && dto.PatientID.Value > 0 && dto.PatientID != prescription.PatientID)
            {
                var patient = await _repo.Patients.GetByIdAsync(dto.PatientID.Value, track: false);
                if (patient == null)
                    return new ResponseDto<PrescriptionDto>("Patient not found.", true);

                prescription.PatientID = dto.PatientID.Value;
            }

            if (dto.Date.HasValue)
                prescription.Date = dto.Date.Value;
            if (!string.IsNullOrWhiteSpace(dto.Notes))
                prescription.Notes = dto.Notes;

            _repo.Prescriptions.Update(prescription);
            await _repo.SaveChangesAsync();

            var resultDto = prescription.Adapt<PrescriptionDto>();
            return new ResponseDto<PrescriptionDto>(resultDto);
        }

        // DeleteAsync
        public async Task<ResponseDto<bool>> DeleteAsync(int id, CancellationToken ct = default)
        {
            if (id <= 0)
                return new ResponseDto<bool>(MsgResponce.Failed, true);

            var prescription = await _repo.Prescriptions.GetByIdAsync(id, track: true);
            if (prescription == null)
                return new ResponseDto<bool>(MsgResponce.Failed, true);

            _repo.Prescriptions.Remove(prescription);
            await _repo.SaveChangesAsync();

            return new ResponseDto<bool>(true);
        }
    }
}
