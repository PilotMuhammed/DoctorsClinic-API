using DoctorsClinic.Core.Dtos;
using DoctorsClinic.Core.Dtos.Doctors;
using DoctorsClinic.Core.Extensions;
using DoctorsClinic.Core.Helper;
using DoctorsClinic.Core.IServices;
using DoctorsClinic.Infrastructure.IRepositories;
using Mapster;
using Microsoft.EntityFrameworkCore;

namespace DoctorsClinic.Core.Services
{
    public class DoctorService : IDoctorService
    {
        private readonly IRepositoryWrapper _repo;

        public DoctorService(IRepositoryWrapper repo)
        {
            _repo = repo;
        }

        // GetAllAsync 
        public async Task<ResponseDto<PaginationDto<DoctorDto>>> GetAllAsync(
            PaginationQuery pagination,
            DoctorFilterDto filter,
            CancellationToken ct = default)
        {
            if (pagination == null)
                return new ResponseDto<PaginationDto<DoctorDto>>(MsgResponce.Failed, true);

            var query = _repo.Doctors.GetAll(include: q =>
                q.Include(d => d.Specialty!), track: false);

            // Apply Filter
            if (filter != null)
            {
                if (!string.IsNullOrWhiteSpace(filter.FullName))
                    query = query.Where(d => d.FullName.Contains(filter.FullName));
                if (filter.SpecialtyID.HasValue)
                    query = query.Where(d => d.SpecialtyID == filter.SpecialtyID.Value);
                if (!string.IsNullOrWhiteSpace(filter.Email))
                    query = query.Where(d => d.Email!.Contains(filter.Email));
            }

            var totalCount = await query.CountAsync(ct);
            var data = await query
                .ApplyPagging(pagination)
                .ProjectToType<DoctorDto>()
                .ToListAsync(ct);

            var meta = new PaginationMetadata(totalCount, pagination);
            var paginated = new PaginationDto<DoctorDto>(data, meta);

            if (!data.Any())
                return new ResponseDto<PaginationDto<DoctorDto>>(MsgResponce.Doctor.NotFound, true);

            return new ResponseDto<PaginationDto<DoctorDto>>(paginated);
        }

        // GetListAsync 
        public async Task<ResponseDto<IEnumerable<ListDto<int>>>> GetListAsync(CancellationToken ct = default)
        {
            var ids = await _repo.Doctors.GetAll(track: false)
                .Select(d => d.DoctorID)
                .ToListAsync(ct);

            if (!ids.Any())
                return new ResponseDto<IEnumerable<ListDto<int>>>(MsgResponce.Doctor.NotFound, true);

            var listDto = new ListDto<int>
            {
                Items = ids,
                TotalCount = ids.Count
            };

            return new ResponseDto<IEnumerable<ListDto<int>>>(new List<ListDto<int>> { listDto });
        }

        // GetBySpecialtyAsync 
        public async Task<ResponseDto<List<DoctorDto>>> GetBySpecialtyAsync(int specialtyId, CancellationToken ct = default)
        {
            if (specialtyId <= 0)
                return new ResponseDto<List<DoctorDto>>(MsgResponce.Specialty.NotFound, true);

            var doctors = await _repo.Doctors
                .FindByCondition(d => d.SpecialtyID == specialtyId, include: q =>
                    q.Include(d => d.Specialty!), track: false)
                .ProjectToType<DoctorDto>()
                .ToListAsync(ct);

            if (!doctors.Any())
                return new ResponseDto<List<DoctorDto>>(MsgResponce.Doctor.NotFound, true);

            return new ResponseDto<List<DoctorDto>>(doctors);
        }

        // GetByIdAsync 
        public async Task<ResponseDto<DoctorResponseDto>> GetByIdAsync(int id, CancellationToken ct = default)
        {
            if (id <= 0)
                return new ResponseDto<DoctorResponseDto>(MsgResponce.Doctor.NotFound, true);

            var doctor = await _repo.Doctors.GetByIdAsync(id, include: q =>
                q.Include(d => d.Specialty!), track: false);

            if (doctor == null)
                return new ResponseDto<DoctorResponseDto>(MsgResponce.Doctor.NotFound, true);

            var dto = doctor.Adapt<DoctorResponseDto>();
            return new ResponseDto<DoctorResponseDto>(dto);
        }

        // CreateAsync
        public async Task<ResponseDto<DoctorDto>> CreateAsync(CreateDoctorDto dto, CancellationToken ct = default)
        {
            if (dto == null)
                return new ResponseDto<DoctorDto>(MsgResponce.Failed, true);
            // Check specialty
            var specialty = await _repo.Specialties.GetByIdAsync(dto.SpecialtyID, track: false);
            if (specialty == null)
                return new ResponseDto<DoctorDto>(MsgResponce.Specialty.NotFound, true);

            // Create Doctor
            var doctor = dto.Adapt<Domain.Entities.Doctor>();
            await _repo.Doctors.AddAsync(doctor);
            await _repo.SaveChangesAsync();

            var resultDto = doctor.Adapt<DoctorDto>();
            return new ResponseDto<DoctorDto>(resultDto);
        }

        // UpdateAsync 
        public async Task<ResponseDto<DoctorDto>> UpdateAsync(int id, UpdateDoctorDto dto, CancellationToken ct = default)
        {
            if (id <= 0 || dto == null)
                return new ResponseDto<DoctorDto>(MsgResponce.Failed, true);

            var doctor = await _repo.Doctors.GetByIdAsync(id, track: true);
            if (doctor == null)
                return new ResponseDto<DoctorDto>(MsgResponce.Doctor.NotFound, true);

            // Check Specialty
            if (dto.SpecialtyID.HasValue && dto.SpecialtyID.Value > 0 && dto.SpecialtyID != doctor.SpecialtyID)
            {
                var specialty = await _repo.Specialties.GetByIdAsync(dto.SpecialtyID.Value, track: false);
                if (specialty == null)
                    return new ResponseDto<DoctorDto>(MsgResponce.Specialty.NotFound, true);

                doctor.SpecialtyID = dto.SpecialtyID.Value;
            }

            // Update
            if (!string.IsNullOrWhiteSpace(dto.FullName))
                doctor.FullName = dto.FullName;
            if (!string.IsNullOrWhiteSpace(dto.Phone))
                doctor.Phone = dto.Phone;

            _repo.Doctors.Update(doctor);
            await _repo.SaveChangesAsync();

            var resultDto = doctor.Adapt<DoctorDto>();
            return new ResponseDto<DoctorDto>(resultDto);
        }

        // DeleteAsync
        public async Task<ResponseDto<bool>> DeleteAsync(int id, CancellationToken ct = default)
        {
            if (id <= 0)
                return new ResponseDto<bool>(MsgResponce.Doctor.NotFound, true);

            var doctor = await _repo.Doctors.GetByIdAsync(id, track: true);
            if (doctor == null)
                return new ResponseDto<bool>(MsgResponce.Doctor.NotFound, true);

            _repo.Doctors.Remove(doctor);
            await _repo.SaveChangesAsync();

            return new ResponseDto<bool>(true);
        }
    }
}
