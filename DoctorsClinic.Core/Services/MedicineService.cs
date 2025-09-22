using DoctorsClinic.Core.Dtos;
using DoctorsClinic.Core.Dtos.Medicines;
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
    public class MedicineService : IMedicineService
    {
        private readonly IRepositoryWrapper _wrapper;
        private readonly IUserAccessorService _userAccessor;
        public MedicineService(IRepositoryWrapper wrapper, IUserAccessorService userAccessor)
        {
            _wrapper = wrapper;
            _userAccessor = userAccessor;
        }

        public async Task<ResponseDto<PaginationDto<MedicineDto>>> GetAll(PaginationQuery paginationQuery, MedicineFilterDto filter)
        {
            #region Apply Filter
            var query = _wrapper.MedicineRepo.GetAll()
                .Where(me => string.IsNullOrEmpty(filter.Name) || me.Name.ToLower().Contains(filter.Name.ToLower()))
                .Where(me => string.IsNullOrEmpty(filter.Description) || me.Description.ToLower().Contains(filter.Description.ToLower()))
                .Where(me => string.IsNullOrEmpty(filter.Type) || me.Type.ToLower().Contains(filter.Type.ToLower()));
            #endregion

            var data = await query
                .OrderByDescending(me => me.CreatedAt)
                .ApplyPagging(paginationQuery)
                .ProjectToType<MedicineDto>()
                .ToListAsync();

            var count = await query.CountAsync();
            var metadata = new PaginationMetadata(count, paginationQuery);

            return new ResponseDto<PaginationDto<MedicineDto>>(
                new PaginationDto<MedicineDto>(data, metadata));
        }

        public async Task<ResponseDto<IEnumerable<ListDto<int>>>> GetList()
        {
            var query = _wrapper.MedicineRepo.GetAll();

            return new ResponseDto<IEnumerable<ListDto<int>>>(
                await query.OrderBy(me => me.Name)
                .Select(me => new ListDto<int>
                {
                    Id = me.Id,
                    Title = me.Name
                }).ToListAsync()
            );
        }

        public async Task<ResponseDto<MedicineResponseDto>> GetById(int id)
        {
            var medicine = await _wrapper.MedicineRepo.FindByCondition(me => me.Id == id)
                .Include(me => me.PrescriptionMedicines)
                .FirstOrDefaultAsync();

            if (medicine == null)
                return new ResponseDto<MedicineResponseDto>(MsgResponce.Medicine.NotFound, true);

            return new ResponseDto<MedicineResponseDto>(medicine.Adapt<MedicineResponseDto>());
        }

        public async Task<ResponseDto<MedicineDto>> Add(CreateMedicineDto form)
        {
            var medicine = form.Adapt<Medicine>();
            medicine.CreatorId = _userAccessor.UserId;
            medicine.CreatedAt = DateTime.Now;

            await _wrapper.MedicineRepo.Insert(medicine);
            await _wrapper.SaveAllAsync();

            medicine = await _wrapper.MedicineRepo.FindByCondition(me => me.Id == medicine.Id)
                .FirstOrDefaultAsync();

            return new ResponseDto<MedicineDto>(medicine.Adapt<MedicineDto>());
        }

        public async Task<ResponseDto<MedicineDto>> Update(int id, UpdateMedicineDto form)
        {
            var medicine = await _wrapper.MedicineRepo.FindByCondition(me => me.Id == id)
                .FirstOrDefaultAsync();
            if (medicine == null)
                return new ResponseDto<MedicineDto>(MsgResponce.Medicine.NotFound, true);

            var saveMedicine = form.Adapt(medicine);
            saveMedicine.ModifierId = _userAccessor.UserId;
            saveMedicine.ModifieAt = DateTime.Now;

            await _wrapper.MedicineRepo.Update(saveMedicine);
            return new ResponseDto<MedicineDto>(saveMedicine.Adapt<MedicineDto>());
        }

        public async Task<ResponseDto<bool>> Delete(int id)
        {
            var medicine = await _wrapper.MedicineRepo.FindItemByCondition(me => me.Id == id);
            if (medicine == null)
                return new ResponseDto<bool>(MsgResponce.Medicine.NotFound, true);

            medicine.DeleterId = _userAccessor.UserId;
            await _wrapper.MedicineRepo.Delete(id);
            await _wrapper.SaveAllAsync();
            return new ResponseDto<bool>(true);
        }
    }
}
