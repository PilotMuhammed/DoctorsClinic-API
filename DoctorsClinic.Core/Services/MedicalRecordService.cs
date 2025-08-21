using DoctorsClinic.Core.Dtos;
using DoctorsClinic.Core.Dtos.MedicalRecords;
using DoctorsClinic.Core.Extensions;
using DoctorsClinic.Core.Helper;
using DoctorsClinic.Core.IServices;
using DoctorsClinic.Infrastructure.IRepositories;
using Mapster;
using Microsoft.EntityFrameworkCore;

namespace DoctorsClinic.Core.Services
{
    public class MedicalRecordService : IMedicalRecordService
    {
        private readonly IRepositoryWrapper _repo;

        public MedicalRecordService(IRepositoryWrapper repo)
        {
            _repo = repo;
        }

        // GetAllAsync 
        public async Task<ResponseDto<PaginationDto<MedicalRecordDto>>> GetAllAsync(
            PaginationQuery pagination,
            MedicalRecordFilterDto filter,
            CancellationToken ct = default)
        {
            if (pagination == null)
                return new ResponseDto<PaginationDto<MedicalRecordDto>>(MsgResponce.Failed, true);

            var query = _repo.MedicalRecords.GetAll(include: q =>
                q.Include(m => m.Patient!)
                 .Include(m => m.Doctor!),
                track: false);

            if (filter != null)
            {
                if (filter.PatientID.HasValue)
                    query = query.Where(m => m.PatientID == filter.PatientID.Value);
                if (filter.DoctorID.HasValue)
                    query = query.Where(m => m.DoctorID == filter.DoctorID.Value);
                if (filter.DateFrom.HasValue)
                    query = query.Where(m => m.Date >= filter.DateFrom.Value);
                if (filter.DateTo.HasValue)
                    query = query.Where(m => m.Date <= filter.DateTo.Value);
                if (!string.IsNullOrWhiteSpace(filter.Diagnosis))
                    query = query.Where(m => m.Diagnosis.Contains(filter.Diagnosis));
            }

            var totalCount = await query.CountAsync(ct);
            var data = await query
                .ApplyPagging(pagination)
                .ProjectToType<MedicalRecordDto>()
                .ToListAsync(ct);

            var meta = new PaginationMetadata(totalCount, pagination);
            var paginated = new PaginationDto<MedicalRecordDto>(data, meta);

            if (!data.Any())
                return new ResponseDto<PaginationDto<MedicalRecordDto>>(MsgResponce.MedicalRecord.NotFound, true);

            return new ResponseDto<PaginationDto<MedicalRecordDto>>(paginated);
        }

        // GetByPatientAsync
        public async Task<ResponseDto<List<MedicalRecordDto>>> GetByPatientAsync(int patientId, CancellationToken ct = default)
        {
            if (patientId <= 0)
                return new ResponseDto<List<MedicalRecordDto>>(MsgResponce.Patient.NotFound, true);

            var records = await _repo.MedicalRecords
                .FindByCondition(m => m.PatientID == patientId, include: q =>
                    q.Include(m => m.Doctor!), track: false)
                .ProjectToType<MedicalRecordDto>()
                .ToListAsync(ct);

            if (!records.Any())
                return new ResponseDto<List<MedicalRecordDto>>(MsgResponce.MedicalRecord.NotFound, true);

            return new ResponseDto<List<MedicalRecordDto>>(records);
        }

        // GetByIdAsync
        public async Task<ResponseDto<MedicalRecordResponseDto>> GetByIdAsync(int id, CancellationToken ct = default)
        {
            if (id <= 0)
                return new ResponseDto<MedicalRecordResponseDto>(MsgResponce.MedicalRecord.NotFound, true);

            var record = await _repo.MedicalRecords.GetByIdAsync(id, include: q =>
                q.Include(m => m.Patient!)
                 .Include(m => m.Doctor!), track: false);

            if (record == null)
                return new ResponseDto<MedicalRecordResponseDto>(MsgResponce.MedicalRecord.NotFound, true);

            // Convert Entities to Dtos
            var recordDto = record.Adapt<MedicalRecordDto>();
            var patientDto = record.Patient?.Adapt<Core.Dtos.Patients.PatientDto>();
            var doctorDto = record.Doctor?.Adapt<Core.Dtos.Doctors.DoctorDto>();

            var response = new MedicalRecordResponseDto
            {
                MedicalRecord = recordDto,
                Patient = patientDto,
                Doctor = doctorDto
            };

            return new ResponseDto<MedicalRecordResponseDto>(response);
        }

        // CreateAsync 
        public async Task<ResponseDto<MedicalRecordDto>> CreateAsync(CreateMedicalRecordDto dto, CancellationToken ct = default)
        {
            if (dto == null)
                return new ResponseDto<MedicalRecordDto>(MsgResponce.Failed, true);

            // Check Patient
            var patient = await _repo.Patients.GetByIdAsync(dto.PatientID, track: false);
            if (patient == null)
                return new ResponseDto<MedicalRecordDto>(MsgResponce.Patient.NotFound, true);
            // Check Doctor
            var doctor = await _repo.Doctors.GetByIdAsync(dto.DoctorID, track: false);
            if (doctor == null)
                return new ResponseDto<MedicalRecordDto>(MsgResponce.Doctor.NotFound, true);

            var record = dto.Adapt<Domain.Entities.MedicalRecord>();
            await _repo.MedicalRecords.AddAsync(record);
            await _repo.SaveChangesAsync();

            var resultDto = record.Adapt<MedicalRecordDto>();
            return new ResponseDto<MedicalRecordDto>(resultDto);
        }

        // UpdateAsync
        public async Task<ResponseDto<MedicalRecordDto>> UpdateAsync(int id, UpdateMedicalRecordDto dto, CancellationToken ct = default)
        {
            if (id <= 0 || dto == null)
                return new ResponseDto<MedicalRecordDto>(MsgResponce.Failed, true);

            var record = await _repo.MedicalRecords.GetByIdAsync(id, track: true);
            if (record == null)
                return new ResponseDto<MedicalRecordDto>(MsgResponce.MedicalRecord.NotFound, true);

            // Update
            if (dto.PatientID.HasValue && dto.PatientID.Value > 0 && dto.PatientID != record.PatientID)
            {
                var patient = await _repo.Patients.GetByIdAsync(dto.PatientID.Value, track: false);
                if (patient == null)
                    return new ResponseDto<MedicalRecordDto>(MsgResponce.Patient.NotFound, true);

                record.PatientID = dto.PatientID.Value;
            }

            if (dto.DoctorID.HasValue && dto.DoctorID.Value > 0 && dto.DoctorID != record.DoctorID)
            {
                var doctor = await _repo.Doctors.GetByIdAsync(dto.DoctorID.Value, track: false);
                if (doctor == null)
                    return new ResponseDto<MedicalRecordDto>(MsgResponce.Doctor.NotFound, true);

                record.DoctorID = dto.DoctorID.Value;
            }

            if (!string.IsNullOrWhiteSpace(dto.Diagnosis))
                record.Diagnosis = dto.Diagnosis;
            if (dto.Date.HasValue)
                record.Date = dto.Date.Value;
            if (!string.IsNullOrWhiteSpace(dto.Notes))
                record.Notes = dto.Notes;

            _repo.MedicalRecords.Update(record);
            await _repo.SaveChangesAsync();

            var resultDto = record.Adapt<MedicalRecordDto>();
            return new ResponseDto<MedicalRecordDto>(resultDto);
        }

        // DeleteAsync
        public async Task<ResponseDto<bool>> DeleteAsync(int id, CancellationToken ct = default)
        {
            if (id <= 0)
                return new ResponseDto<bool>(MsgResponce.MedicalRecord.NotFound, true);

            var record = await _repo.MedicalRecords.GetByIdAsync(id, track: true);
            if (record == null)
                return new ResponseDto<bool>(MsgResponce.MedicalRecord.NotFound, true);

            _repo.MedicalRecords.Remove(record);
            await _repo.SaveChangesAsync();

            return new ResponseDto<bool>(true);
        }
    }
}
