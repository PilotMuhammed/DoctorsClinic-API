using DoctorsClinic.Core.Dtos;
using DoctorsClinic.Core.Dtos.PrescriptionMedicines;
using DoctorsClinic.Core.Extensions;
using DoctorsClinic.Core.Helper;
using DoctorsClinic.Core.IServices;
using DoctorsClinic.Core.IServices.Account;
using DoctorsClinic.Domain.Entities;
using DoctorsClinic.Infrastructure.IRepositories;
using Mapster;
using Microsoft.EntityFrameworkCore;

namespace DoctorsClinic.Core.Services
{
    public class PrescriptionMedicineService : IPrescriptionMedicineService
    {
        private readonly IRepositoryWrapper _wrapper;
        private readonly IUserAccessorService _userAccessor;
        public PrescriptionMedicineService(IRepositoryWrapper wrapper, IUserAccessorService userAccessor)
        {
            _wrapper = wrapper;
            _userAccessor = userAccessor;
        }

        public async Task<ResponseDto<PaginationDto<PrescriptionMedicineDto>>> GetAll(PaginationQuery paginationQuery, PrescriptionMedicineFilterDto filter)
        {
            #region Apply Filter
            var query = _wrapper.PrescriptionMedicineRepo.GetAll()
                .Include(pm => pm.Prescription)
                .Include(pm => pm.Medicine)
                .Where(pm => !filter.PrescriptionID.HasValue || pm.PrescriptionID == filter.PrescriptionID)
                .Where(pm => !filter.MedicineID.HasValue || pm.MedicineID == filter.MedicineID)
                .Where(pm => string.IsNullOrEmpty(filter.Dose) || pm.Dose.ToLower().Contains(filter.Dose.ToLower()))
                .Where(pm => string.IsNullOrEmpty(filter.Duration) || pm.Duration.ToLower().Contains(filter.Duration.ToLower()))
                .Where(pm => string.IsNullOrEmpty(filter.Instructions) || pm.Instructions.ToLower().Contains(filter.Instructions.ToLower()));
            #endregion

            var data = await query
                .OrderByDescending(pm => pm.CreatedAt)
                .ApplyPagging(paginationQuery)
                .ProjectToType<PrescriptionMedicineDto>()
                .ToListAsync();

            var count = await query.CountAsync();
            var metadata = new PaginationMetadata(count, paginationQuery);

            return new ResponseDto<PaginationDto<PrescriptionMedicineDto>>(
                new PaginationDto<PrescriptionMedicineDto>(data, metadata));
        }

        public async Task<ResponseDto<PrescriptionMedicineResponseDto>> GetById(int id)
        {
            var prescriptionMedicine = await _wrapper.PrescriptionMedicineRepo.FindByCondition(pm => pm.Id == id)
                .Include(pm => pm.Medicine)
                .FirstOrDefaultAsync();

            if (prescriptionMedicine == null)
                return new ResponseDto<PrescriptionMedicineResponseDto>(MsgResponce.PrescriptionMedicine.NotFound, true);

            return new ResponseDto<PrescriptionMedicineResponseDto>(prescriptionMedicine.Adapt<PrescriptionMedicineResponseDto>());
        }

        public async Task<ResponseDto<PrescriptionMedicineDto>> Add(CreatePrescriptionMedicineDto form)
        {
            var prescription = await _wrapper.PrescriptionRepo.FindItemByCondition(pr => pr.Id == form.PrescriptionID);
            if (prescription == null)
                return new ResponseDto<PrescriptionMedicineDto>(MsgResponce.Prescription.NotFound, true);

            var medicine = await _wrapper.MedicineRepo.FindItemByCondition(me => me.Id == form.MedicineID);
            if (medicine == null)
                return new ResponseDto<PrescriptionMedicineDto>(MsgResponce.Medicine.NotFound, true);

            var prescriptionMedicine = form.Adapt<PrescriptionMedicine>();
            prescriptionMedicine.CreatorId = _userAccessor.UserId;
            prescriptionMedicine.CreatedAt = DateTime.Now;

            await _wrapper.PrescriptionMedicineRepo.Insert(prescriptionMedicine);
            await _wrapper.SaveAllAsync();

            prescriptionMedicine = await _wrapper.PrescriptionMedicineRepo.FindByCondition(pm => pm.Id == prescriptionMedicine.Id)
                .Include(pm => pm.Prescription)
                .Include(pm => pm.Medicine)
                .FirstOrDefaultAsync();

            return new ResponseDto<PrescriptionMedicineDto>(prescriptionMedicine.Adapt<PrescriptionMedicineDto>());
        }

        public async Task<ResponseDto<PrescriptionMedicineDto>> Update(int id, UpdatePrescriptionMedicineDto form)
        {
            var prescriptionMedicine = await _wrapper.PrescriptionMedicineRepo.FindByCondition(pm => pm.Id == id)
                .Include(pm => pm.Prescription)
                .Include(pm => pm.Medicine)
                .FirstOrDefaultAsync();
            if (prescriptionMedicine == null)
                return new ResponseDto<PrescriptionMedicineDto>(MsgResponce.PrescriptionMedicine.NotFound, true);

            var prescription = await _wrapper.PrescriptionRepo.FindItemByCondition(pr => pr.Id == form.PrescriptionID);
            if (prescription == null)
                return new ResponseDto<PrescriptionMedicineDto>(MsgResponce.Prescription.NotFound, true);

            var medicine = await _wrapper.MedicineRepo.FindItemByCondition(me => me.Id == form.MedicineID);
            if (medicine == null)
                return new ResponseDto<PrescriptionMedicineDto>(MsgResponce.Medicine.NotFound, true);

            var savePrescriptionMedicine = form.Adapt(prescriptionMedicine);
            savePrescriptionMedicine.ModifierId = _userAccessor.UserId;
            savePrescriptionMedicine.ModifieAt = DateTime.Now;

            await _wrapper.PrescriptionMedicineRepo.Update(savePrescriptionMedicine);
            return new ResponseDto<PrescriptionMedicineDto>(savePrescriptionMedicine.Adapt<PrescriptionMedicineDto>());
        }

        public async Task<ResponseDto<bool>> Delete(int id)
        {
            var prescriptionMedicine = await _wrapper.PrescriptionMedicineRepo.FindItemByCondition(pm => pm.Id == id);
            if (prescriptionMedicine == null)
                return new ResponseDto<bool>(MsgResponce.PrescriptionMedicine.NotFound, true);

            prescriptionMedicine.DeleterId = _userAccessor.UserId;
            await _wrapper.PrescriptionMedicineRepo.Delete(id);
            await _wrapper.SaveAllAsync();
            return new ResponseDto<bool>(true);
        }
    }
}
