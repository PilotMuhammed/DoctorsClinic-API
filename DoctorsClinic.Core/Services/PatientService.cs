using DoctorsClinic.Core.Dtos;
using DoctorsClinic.Core.Dtos.Patients;
using DoctorsClinic.Core.Extensions;
using DoctorsClinic.Core.Helper;
using DoctorsClinic.Core.IServices;
using DoctorsClinic.Infrastructure.IRepositories;
using Mapster;
using Microsoft.EntityFrameworkCore;
using DoctorsClinic.Domain.Enums;

namespace DoctorsClinic.Core.Services
{
    public class PatientService : IPatientService
    {
        private readonly IRepositoryWrapper _repo;

        public PatientService(IRepositoryWrapper repo)
        {
            _repo = repo;
        }

        // GetAllAsync 
        public async Task<ResponseDto<PaginationDto<PatientDto>>> GetAllAsync(
            PaginationQuery pagination,
            PatientFilterDto filter,
            CancellationToken ct = default)
        {
            if (pagination == null)
                return new ResponseDto<PaginationDto<PatientDto>>(MsgResponce.Failed, true);

            var query = _repo.Patients.GetAll(track: false);

            if (filter != null)
            {
                if (!string.IsNullOrWhiteSpace(filter.FullName))
                    query = query.Where(p => p.FullName.Contains(filter.FullName));

                if (!string.IsNullOrWhiteSpace(filter.Gender))
                    if(Enum.TryParse<Domain.Enums.Gender>(filter.Gender, out var genderEnum))
                        query = query.Where(p => p.Gender == genderEnum);

                if (filter.DOBFrom.HasValue)
                    query = query.Where(p => p.DOB >= filter.DOBFrom.Value);
                if (filter.DOBTo.HasValue)
                    query = query.Where(p => p.DOB <= filter.DOBTo.Value);
                if (!string.IsNullOrWhiteSpace(filter.Phone))
                    query = query.Where(p => p.Phone == filter.Phone);
                if (!string.IsNullOrWhiteSpace(filter.Email))
                    query = query.Where(p => p.Email == filter.Email);
            }

            var totalCount = await query.CountAsync(ct);
            var data = await query
                .ApplyPagging(pagination)
                .ProjectToType<PatientDto>()
                .ToListAsync(ct);

            var meta = new PaginationMetadata(totalCount, pagination);
            var paginated = new PaginationDto<PatientDto>(data, meta);

            if (!data.Any())
                return new ResponseDto<PaginationDto<PatientDto>>(MsgResponce.Patient.NotFound, true);

            return new ResponseDto<PaginationDto<PatientDto>>(paginated);
        }

        // GetListAsync
        public async Task<ResponseDto<IEnumerable<ListDto<int>>>> GetListAsync(CancellationToken ct = default)
        {
            var ids = await _repo.Patients.GetAll(track: false)
                .Select(p => p.PatientID)
                .ToListAsync(ct);

            if (!ids.Any())
                return new ResponseDto<IEnumerable<ListDto<int>>>(MsgResponce.Patient.NotFound, true);

            var listDto = new ListDto<int>
            {
                Items = ids,
                TotalCount = ids.Count
            };

            return new ResponseDto<IEnumerable<ListDto<int>>>(new List<ListDto<int>> { listDto });
        }

        // GetByIdAsync
        public async Task<ResponseDto<PatientResponseDto>> GetByIdAsync(int id, CancellationToken ct = default)
        {
            if (id <= 0)
                return new ResponseDto<PatientResponseDto>(MsgResponce.Patient.NotFound, true);

            var patient = await _repo.Patients.GetWithAllDetailsAsync(id);

            if (patient == null)
                return new ResponseDto<PatientResponseDto>(MsgResponce.Patient.NotFound, true);

            var patientDto = patient.Adapt<PatientDto>();

            var appointments = patient.Appointments?.Select(a => a.Adapt<Core.Dtos.Appointments.AppointmentDto>()).ToList() ?? new();
            var medicalRecords = patient.MedicalRecords?.Select(r => r.Adapt<Core.Dtos.MedicalRecords.MedicalRecordDto>()).ToList() ?? new();
            var prescriptions = patient.Prescriptions?.Select(pr => pr.Adapt<Core.Dtos.Prescriptions.PrescriptionDto>()).ToList() ?? new();
            var invoices = patient.Invoices?.Select(inv => inv.Adapt<Core.Dtos.Invoices.InvoiceDto>()).ToList() ?? new();

            var response = new PatientResponseDto
            {
                Patient = patientDto,
                Appointments = appointments,
                MedicalRecords = medicalRecords,
                Prescriptions = prescriptions,
                Invoices = invoices
            };

            return new ResponseDto<PatientResponseDto>(response);
        }

        // CreateAsync
        public async Task<ResponseDto<PatientDto>> CreateAsync(CreatePatientDto dto, CancellationToken ct = default)
        {
            if (dto == null)
                return new ResponseDto<PatientDto>(MsgResponce.Failed, true);

            if (string.IsNullOrWhiteSpace(dto.FullName))
                return new ResponseDto<PatientDto>("Patient full name is required.", true);

            if (string.IsNullOrWhiteSpace(dto.Gender))
                return new ResponseDto<PatientDto>("Patient gender is required.", true);


            var patient = dto.Adapt<Domain.Entities.Patient>();
            await _repo.Patients.AddAsync(patient);
            await _repo.SaveChangesAsync();

            var resultDto = patient.Adapt<PatientDto>();
            return new ResponseDto<PatientDto>(resultDto);
        }

        // UpdateAsync 
        public async Task<ResponseDto<PatientDto>> UpdateAsync(int id, UpdatePatientDto dto, CancellationToken ct = default)
        {
            if (id <= 0 || dto == null)
                return new ResponseDto<PatientDto>(MsgResponce.Failed, true);

            var patient = await _repo.Patients.GetByIdAsync(id, track: true);
            if (patient == null)
                return new ResponseDto<PatientDto>(MsgResponce.Patient.NotFound, true);

            if (!string.IsNullOrWhiteSpace(dto.FullName))
                patient.FullName = dto.FullName;

            if (!string.IsNullOrWhiteSpace(dto.Gender))
                if(Enum.TryParse<Domain.Enums.Gender>(dto.Gender, out var genderEnum))
                patient.Gender = genderEnum;

            if (dto.DOB.HasValue)
                patient.DOB = dto.DOB.Value;
            if (!string.IsNullOrWhiteSpace(dto.Phone))
                patient.Phone = dto.Phone;

            if (!string.IsNullOrWhiteSpace(dto.Address))
                patient.Address = dto.Address;

            _repo.Patients.Update(patient);
            await _repo.SaveChangesAsync();

            var resultDto = patient.Adapt<PatientDto>();
            return new ResponseDto<PatientDto>(resultDto);
        }

        // DeleteAsync
        public async Task<ResponseDto<bool>> DeleteAsync(int id, CancellationToken ct = default)
        {
            if (id <= 0)
                return new ResponseDto<bool>(MsgResponce.Patient.NotFound, true);

            var patient = await _repo.Patients.GetByIdAsync(id, track: true);
            if (patient == null)
                return new ResponseDto<bool>(MsgResponce.Patient.NotFound, true);

            _repo.Patients.Remove(patient);
            await _repo.SaveChangesAsync();

            return new ResponseDto<bool>(true);
        }
    }
}
