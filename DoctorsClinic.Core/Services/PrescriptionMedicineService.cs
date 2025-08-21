using DoctorsClinic.Core.Dtos;
using DoctorsClinic.Core.Dtos.PrescriptionMedicines;
using DoctorsClinic.Core.Extensions;
using DoctorsClinic.Core.Helper;
using DoctorsClinic.Core.IServices;
using DoctorsClinic.Infrastructure.IRepositories;
using Mapster;
using Microsoft.EntityFrameworkCore;

namespace DoctorsClinic.Core.Services
{
    public class PrescriptionMedicineService : IPrescriptionMedicineService
    {
        private readonly IRepositoryWrapper _repo;

        public PrescriptionMedicineService(IRepositoryWrapper repo)
        {
            _repo = repo;
        }

        // GetAllAsync
        public async Task<ResponseDto<PaginationDto<PrescriptionMedicineDto>>> GetAllAsync(
            PaginationQuery pagination,
            PrescriptionMedicineFilterDto filter,
            CancellationToken ct = default)
        {
            if (pagination == null)
                return new ResponseDto<PaginationDto<PrescriptionMedicineDto>>(MsgResponce.Failed, true);

            var query = _repo.PrescriptionMedicines.GetAll(track: false);

            if (filter != null)
            {
                if (filter.PrescriptionID.HasValue)
                    query = query.Where(pm => pm.PrescriptionID == filter.PrescriptionID.Value);
                if (filter.MedicineID.HasValue)
                    query = query.Where(pm => pm.MedicineID == filter.MedicineID.Value);
                if (!string.IsNullOrWhiteSpace(filter.Dose))
                    query = query.Where(pm => pm.Dose.Contains(filter.Dose));
                if (!string.IsNullOrWhiteSpace(filter.Duration))
                    query = query.Where(pm => pm.Duration.Contains(filter.Duration));
                if (!string.IsNullOrWhiteSpace(filter.Instructions))
                    query = query.Where(pm => pm.Instructions.Contains(filter.Instructions));
            }

            var totalCount = await query.CountAsync(ct);
            var data = await query
                .ApplyPagging(pagination)
                .ProjectToType<PrescriptionMedicineDto>()
                .ToListAsync(ct);

            var meta = new PaginationMetadata(totalCount, pagination);
            var paginated = new PaginationDto<PrescriptionMedicineDto>(data, meta);

            if (!data.Any())
                return new ResponseDto<PaginationDto<PrescriptionMedicineDto>>(MsgResponce.Failed, true);

            return new ResponseDto<PaginationDto<PrescriptionMedicineDto>>(paginated);
        }

        // GetByPrescriptionAsync 
        public async Task<ResponseDto<List<PrescriptionMedicineDto>>> GetByPrescriptionAsync(
            int prescriptionId, CancellationToken ct = default)
        {
            if (prescriptionId <= 0)
                return new ResponseDto<List<PrescriptionMedicineDto>>(MsgResponce.Failed, true);

            var list = await _repo.PrescriptionMedicines
                .FindByCondition(pm => pm.PrescriptionID == prescriptionId, track: false)
                .ProjectToType<PrescriptionMedicineDto>()
                .ToListAsync(ct);

            if (!list.Any())
                return new ResponseDto<List<PrescriptionMedicineDto>>(MsgResponce.Failed, true);

            return new ResponseDto<List<PrescriptionMedicineDto>>(list);
        }

        // GetByIdAsync
        public async Task<ResponseDto<PrescriptionMedicineResponseDto>> GetByIdAsync(int id, CancellationToken ct = default)
        {
            if (id <= 0)
                return new ResponseDto<PrescriptionMedicineResponseDto>(MsgResponce.Failed, true);

            var item = await _repo.PrescriptionMedicines.GetWithDetailsAsync(id);
            if (item == null)
                return new ResponseDto<PrescriptionMedicineResponseDto>(MsgResponce.Failed, true);

            var dto = item.Adapt<PrescriptionMedicineDto>();


            var response = new PrescriptionMedicineResponseDto
            {
                PrescriptionMedicine = dto
            };

            return new ResponseDto<PrescriptionMedicineResponseDto>(response);
        }

        // CreateAsync
        public async Task<ResponseDto<PrescriptionMedicineDto>> CreateAsync(CreatePrescriptionMedicineDto dto, CancellationToken ct = default)
        {
            if (dto == null)
                return new ResponseDto<PrescriptionMedicineDto>(MsgResponce.Failed, true);

            if (dto.PrescriptionID <= 0)
                return new ResponseDto<PrescriptionMedicineDto>("Invalid prescription ID.", true);

            if (dto.MedicineID <= 0)
                return new ResponseDto<PrescriptionMedicineDto>("Invalid medicine ID.", true);


            var medicine = await _repo.Medicines.GetByIdAsync(dto.MedicineID, track: false);
            if (medicine == null)
                return new ResponseDto<PrescriptionMedicineDto>("Medicine not found.", true);

            var entity = dto.Adapt<Domain.Entities.PrescriptionMedicine>();
            await _repo.PrescriptionMedicines.AddAsync(entity);
            await _repo.SaveChangesAsync();

            var resultDto = entity.Adapt<PrescriptionMedicineDto>();
            return new ResponseDto<PrescriptionMedicineDto>(resultDto);
        }

        // UpdateAsync
        public async Task<ResponseDto<PrescriptionMedicineDto>> UpdateAsync(int id, UpdatePrescriptionMedicineDto dto, CancellationToken ct = default)
        {
            if (id <= 0 || dto == null)
                return new ResponseDto<PrescriptionMedicineDto>(MsgResponce.Failed, true);

            var entity = await _repo.PrescriptionMedicines.GetByIdAsync(id, track: true);
            if (entity == null)
                return new ResponseDto<PrescriptionMedicineDto>(MsgResponce.Failed, true);

            if (dto.PrescriptionID.HasValue && dto.PrescriptionID.Value > 0 && dto.PrescriptionID != entity.PrescriptionID)
            {
                var prescription = await _repo.Prescriptions.GetByIdAsync(dto.PrescriptionID.Value, track: false);
                if (prescription == null)
                    return new ResponseDto<PrescriptionMedicineDto>("Prescription not found.", true);

                entity.PrescriptionID = dto.PrescriptionID.Value;
            }

            if (dto.MedicineID.HasValue && dto.MedicineID.Value > 0 && dto.MedicineID != entity.MedicineID)
            {
                var medicine = await _repo.Medicines.GetByIdAsync(dto.MedicineID.Value, track: false);
                if (medicine == null)
                    return new ResponseDto<PrescriptionMedicineDto>("Medicine not found.", true);

                entity.MedicineID = dto.MedicineID.Value;
            }

            if (!string.IsNullOrWhiteSpace(dto.Dose))
                entity.Dose = dto.Dose;
            if (!string.IsNullOrWhiteSpace(dto.Duration))
                entity.Duration = dto.Duration;
            if (!string.IsNullOrWhiteSpace(dto.Instructions))
                entity.Instructions = dto.Instructions;

            _repo.PrescriptionMedicines.Update(entity);
            await _repo.SaveChangesAsync();

            var resultDto = entity.Adapt<PrescriptionMedicineDto>();
            return new ResponseDto<PrescriptionMedicineDto>(resultDto);
        }

        // DeleteAsync
        public async Task<ResponseDto<bool>> DeleteAsync(int id, CancellationToken ct = default)
        {
            if (id <= 0)
                return new ResponseDto<bool>(MsgResponce.Failed, true);

            var entity = await _repo.PrescriptionMedicines.GetByIdAsync(id, track: true);
            if (entity == null)
                return new ResponseDto<bool>(MsgResponce.Failed, true);

            _repo.PrescriptionMedicines.Remove(entity);
            await _repo.SaveChangesAsync();

            return new ResponseDto<bool>(true);
        }
    }
}
