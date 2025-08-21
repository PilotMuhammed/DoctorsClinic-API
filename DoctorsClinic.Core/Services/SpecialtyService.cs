using DoctorsClinic.Core.Dtos;
using DoctorsClinic.Core.Dtos.Specialties;
using DoctorsClinic.Core.Extensions;
using DoctorsClinic.Core.Helper;
using DoctorsClinic.Core.IServices;
using DoctorsClinic.Infrastructure.IRepositories;
using Mapster;
using Microsoft.EntityFrameworkCore;

namespace DoctorsClinic.Core.Services
{
    public class SpecialtyService : ISpecialtyService
    {
        private readonly IRepositoryWrapper _repo;

        public SpecialtyService(IRepositoryWrapper repo)
        {
            _repo = repo;
        }

        // GetAllAsync 
        public async Task<ResponseDto<PaginationDto<SpecialtyDto>>> GetAllAsync(
            PaginationQuery pagination,
            SpecialtyFilterDto filter,
            CancellationToken ct = default)
        {
            if (pagination == null)
                return new ResponseDto<PaginationDto<SpecialtyDto>>(MsgResponce.Failed, true);

            var query = _repo.Specialties.GetAll(track: false);

            if (filter != null && !string.IsNullOrWhiteSpace(filter.Name))
                query = query.Where(s => s.Name.Contains(filter.Name));

            var totalCount = await query.CountAsync(ct);
            var data = await query
                .ApplyPagging(pagination)
                .ProjectToType<SpecialtyDto>()
                .ToListAsync(ct);

            var meta = new PaginationMetadata(totalCount, pagination);
            var paginated = new PaginationDto<SpecialtyDto>(data, meta);

            if (!data.Any())
                return new ResponseDto<PaginationDto<SpecialtyDto>>(MsgResponce.Failed, true);

            return new ResponseDto<PaginationDto<SpecialtyDto>>(paginated);
        }

        // GetListAsync
        public async Task<ResponseDto<IEnumerable<ListDto<int>>>> GetListAsync(CancellationToken ct = default)
        {
            var ids = await _repo.Specialties.GetAll(track: false)
                .Select(s => s.SpecialtyID)
                .ToListAsync(ct);

            if (!ids.Any())
                return new ResponseDto<IEnumerable<ListDto<int>>>(MsgResponce.Failed, true);

            var listDto = new ListDto<int>
            {
                Items = ids,
                TotalCount = ids.Count
            };

            return new ResponseDto<IEnumerable<ListDto<int>>>(new List<ListDto<int>> { listDto });
        }

        // GetByIdAsync 
        public async Task<ResponseDto<SpecialtyResponseDto>> GetByIdAsync(int id, CancellationToken ct = default)
        {
            if (id <= 0)
                return new ResponseDto<SpecialtyResponseDto>(MsgResponce.Failed, true);

            var specialty = await _repo.Specialties.GetWithDoctorsAsync(id);
            if (specialty == null)
                return new ResponseDto<SpecialtyResponseDto>(MsgResponce.Failed, true);

            var dto = specialty.Adapt<SpecialtyDto>();
            var doctors = specialty.Doctors?
                .Select(d => d.Adapt<Core.Dtos.Doctors.DoctorDto>())
                .ToList() ?? new List<Core.Dtos.Doctors.DoctorDto>();

            var response = new SpecialtyResponseDto
            {
                Specialty = dto,
                Doctors = doctors
            };

            return new ResponseDto<SpecialtyResponseDto>(response);
        }

        // CreateAsync 
        public async Task<ResponseDto<SpecialtyDto>> CreateAsync(CreateSpecialtyDto dto, CancellationToken ct = default)
        {
            if (dto == null)
                return new ResponseDto<SpecialtyDto>(MsgResponce.Failed, true);

            if (string.IsNullOrWhiteSpace(dto.Name))
                return new ResponseDto<SpecialtyDto>("Specialty name is required.", true);

            var specialty = dto.Adapt<Domain.Entities.Specialty>();
            await _repo.Specialties.AddAsync(specialty);
            await _repo.SaveChangesAsync();

            var resultDto = specialty.Adapt<SpecialtyDto>();
            return new ResponseDto<SpecialtyDto>(resultDto);
        }

        // UpdateAsync 
        public async Task<ResponseDto<SpecialtyDto>> UpdateAsync(int id, UpdateSpecialtyDto dto, CancellationToken ct = default)
        {
            if (id <= 0 || dto == null)
                return new ResponseDto<SpecialtyDto>(MsgResponce.Failed, true);

            var specialty = await _repo.Specialties.GetByIdAsync(id, track: true);
            if (specialty == null)
                return new ResponseDto<SpecialtyDto>(MsgResponce.Failed, true);

            if (!string.IsNullOrWhiteSpace(dto.Name) && dto.Name != specialty.Name)
            {
                var exists = await _repo.Specialties
                    .FindByCondition(s => s.Name == dto.Name && s.SpecialtyID != id, track: false)
                    .AnyAsync(ct);

                if (exists)
                    return new ResponseDto<SpecialtyDto>("Specialty name already exists.", true);

                specialty.Name = dto.Name;
            }

            if (!string.IsNullOrWhiteSpace(dto.Description))
                specialty.Description = dto.Description;

            _repo.Specialties.Update(specialty);
            await _repo.SaveChangesAsync();

            var resultDto = specialty.Adapt<SpecialtyDto>();
            return new ResponseDto<SpecialtyDto>(resultDto);
        }

        // DeleteAsync
        public async Task<ResponseDto<bool>> DeleteAsync(int id, CancellationToken ct = default)
        {
            if (id <= 0)
                return new ResponseDto<bool>(MsgResponce.Failed, true);

            var specialty = await _repo.Specialties.GetByIdAsync(id, track: true);
            if (specialty == null)
                return new ResponseDto<bool>(MsgResponce.Failed, true);

            _repo.Specialties.Remove(specialty);
            await _repo.SaveChangesAsync();

            return new ResponseDto<bool>(true);
        }
    }
}
