using DoctorsClinic.Core.Dtos;
using DoctorsClinic.Core.Dtos.Medicines;
using DoctorsClinic.Core.Extensions;
using DoctorsClinic.Core.Helper;
using DoctorsClinic.Core.IServices;
using DoctorsClinic.Infrastructure.IRepositories;
using Mapster;
using Microsoft.EntityFrameworkCore;

namespace DoctorsClinic.Core.Services
{
    public class MedicineService : IMedicineService
    {
        private readonly IRepositoryWrapper _repo;

        public MedicineService(IRepositoryWrapper repo)
        {
            _repo = repo;
        }

        // GetAllAsync 
        public async Task<ResponseDto<PaginationDto<MedicineDto>>> GetAllAsync(
            PaginationQuery pagination,
            MedicineFilterDto filter,
            CancellationToken ct = default)
        {
            if (pagination == null)
                return new ResponseDto<PaginationDto<MedicineDto>>(MsgResponce.Failed, true);

            var query = _repo.Medicines.GetAll(track: false);

            if (filter != null)
            {
                if (!string.IsNullOrWhiteSpace(filter.Name))
                    query = query.Where(m => m.Name.Contains(filter.Name));
                if (!string.IsNullOrWhiteSpace(filter.Type))
                    query = query.Where(m => m.Type == filter.Type);
            }

            var totalCount = await query.CountAsync(ct);
            var data = await query
                .ApplyPagging(pagination)
                .ProjectToType<MedicineDto>()
                .ToListAsync(ct);

            var meta = new PaginationMetadata(totalCount, pagination);
            var paginated = new PaginationDto<MedicineDto>(data, meta);

            if (!data.Any())
                return new ResponseDto<PaginationDto<MedicineDto>>(MsgResponce.Medicine.NotFound, true);

            return new ResponseDto<PaginationDto<MedicineDto>>(paginated);
        }

        // GetListAsync 
        public async Task<ResponseDto<IEnumerable<ListDto<int>>>> GetListAsync(CancellationToken ct = default)
        {
            var ids = await _repo.Medicines.GetAll(track: false)
                .Select(m => m.MedicineID)
                .ToListAsync(ct);

            if (!ids.Any())
                return new ResponseDto<IEnumerable<ListDto<int>>>(MsgResponce.Medicine.NotFound, true);

            var listDto = new ListDto<int>
            {
                Items = ids,
                TotalCount = ids.Count
            };

            return new ResponseDto<IEnumerable<ListDto<int>>>(new List<ListDto<int>> { listDto });
        }

        // GetByIdAsync
        public async Task<ResponseDto<MedicineResponseDto>> GetByIdAsync(int id, CancellationToken ct = default)
        {
            if (id <= 0)
                return new ResponseDto<MedicineResponseDto>(MsgResponce.Medicine.NotFound, true);

            // Get Medicine with Prescription
            var medicine = await _repo.Medicines.GetWithPrescriptionsAsync(id);
            if (medicine == null)
                return new ResponseDto<MedicineResponseDto>(MsgResponce.Medicine.NotFound, true);

            var dto = medicine.Adapt<MedicineDto>();
            var prescriptions = medicine.PrescriptionMedicines?
                .Select(pm => pm.Adapt<Core.Dtos.PrescriptionMedicines.PrescriptionMedicineDto>())
                .ToList() ?? new List<Core.Dtos.PrescriptionMedicines.PrescriptionMedicineDto>();

            var response = new MedicineResponseDto
            {
                Medicine = dto,
                PrescriptionMedicines = prescriptions
            };

            return new ResponseDto<MedicineResponseDto>(response);
        }

        // CreateAsync
        public async Task<ResponseDto<MedicineDto>> CreateAsync(CreateMedicineDto dto, CancellationToken ct = default)
        {
            if (dto == null)
                return new ResponseDto<MedicineDto>(MsgResponce.Failed, true);

            if (string.IsNullOrWhiteSpace(dto.Name))
                return new ResponseDto<MedicineDto>("Medicine name is required.", true);


            var medicine = dto.Adapt<Domain.Entities.Medicine>();
            await _repo.Medicines.AddAsync(medicine);
            await _repo.SaveChangesAsync();

            var resultDto = medicine.Adapt<MedicineDto>();
            return new ResponseDto<MedicineDto>(resultDto);
        }

        // UpdateAsync
        public async Task<ResponseDto<MedicineDto>> UpdateAsync(int id, UpdateMedicineDto dto, CancellationToken ct = default)
        {
            if (id <= 0 || dto == null)
                return new ResponseDto<MedicineDto>(MsgResponce.Failed, true);

            var medicine = await _repo.Medicines.GetByIdAsync(id, track: true);
            if (medicine == null)
                return new ResponseDto<MedicineDto>(MsgResponce.Medicine.NotFound, true);

            if (!string.IsNullOrWhiteSpace(dto.Description))
                medicine.Description = dto.Description;
            if (!string.IsNullOrWhiteSpace(dto.Type))
                medicine.Type = dto.Type;

            _repo.Medicines.Update(medicine);
            await _repo.SaveChangesAsync();

            var resultDto = medicine.Adapt<MedicineDto>();
            return new ResponseDto<MedicineDto>(resultDto);
        }

        // DeleteAsync 
        public async Task<ResponseDto<bool>> DeleteAsync(int id, CancellationToken ct = default)
        {
            if (id <= 0)
                return new ResponseDto<bool>(MsgResponce.Medicine.NotFound, true);

            var medicine = await _repo.Medicines.GetByIdAsync(id, track: true);
            if (medicine == null)
                return new ResponseDto<bool>(MsgResponce.Medicine.NotFound, true);

            _repo.Medicines.Remove(medicine);
            await _repo.SaveChangesAsync();

            return new ResponseDto<bool>(true);
        }
    }
}
