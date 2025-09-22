using DoctorsClinic.Core.Dtos;
using DoctorsClinic.Core.Dtos.Specialties;
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
    public class SpecialtyService : ISpecialtyService
    {
        private readonly IRepositoryWrapper _wrapper;
        private readonly IUserAccessorService _userAccessor;
        public SpecialtyService(IRepositoryWrapper wrapper, IUserAccessorService userAccessor)
        {
            _wrapper = wrapper;
            _userAccessor = userAccessor;
        }

        public async Task<ResponseDto<PaginationDto<SpecialtyDto>>> GetAll(PaginationQuery paginationQuery, SpecialtyFilterDto filter)
        {
            #region Apply Filter
            var query = _wrapper.SpecialtyRepo.GetAll()
                .Where(sp => string.IsNullOrEmpty(filter.Name) || sp.Name.ToLower().Contains(filter.Name.ToLower()))
                .Where(sp => string.IsNullOrEmpty(filter.Description) || sp.Description!.ToLower().Contains(filter.Description.ToLower()));
            #endregion

            var data = await query
                .OrderByDescending(sp => sp.CreatedAt)
                .ApplyPagging(paginationQuery)
                .ProjectToType<SpecialtyDto>()
                .ToListAsync();

            var count = await query.CountAsync();
            var metadata = new PaginationMetadata(count, paginationQuery);

            return new ResponseDto<PaginationDto<SpecialtyDto>>(
                new PaginationDto<SpecialtyDto>(data, metadata));
        }

        public async Task<ResponseDto<IEnumerable<ListDto<int>>>> GetList()
        {
            var query = _wrapper.SpecialtyRepo.GetAll();

            return new ResponseDto<IEnumerable<ListDto<int>>>(
                await query.OrderBy(sp => sp.Name)
                .Select(sp => new ListDto<int>
                {
                    Id = sp.Id,
                    Title = sp.Name
                }).ToListAsync()
            );
        }

        public async Task<ResponseDto<SpecialtyResponseDto>> GetById(int id)
        {
            var specialty = await _wrapper.SpecialtyRepo.FindByCondition(sp => sp.Id == id)
                .Include(sp => sp.Doctors)
                .FirstOrDefaultAsync();

            if (specialty == null)
                return new ResponseDto<SpecialtyResponseDto>(MsgResponce.Specialty.NotFound, true);

            return new ResponseDto<SpecialtyResponseDto>(specialty.Adapt<SpecialtyResponseDto>());
        }

        public async Task<ResponseDto<SpecialtyDto>> Add(CreateSpecialtyDto form)
        {
            var specialty = form.Adapt<Specialty>();
            specialty.CreatorId = _userAccessor.UserId;
            specialty.CreatedAt = DateTime.Now;

            await _wrapper.SpecialtyRepo.Insert(specialty);
            await _wrapper.SaveAllAsync();

            specialty = await _wrapper.SpecialtyRepo.FindByCondition(sp => sp.Id == specialty.Id)
                .FirstOrDefaultAsync();

            return new ResponseDto<SpecialtyDto>(specialty.Adapt<SpecialtyDto>());
        }

        public async Task<ResponseDto<SpecialtyDto>> Update(int id, UpdateSpecialtyDto form)
        {
            var specialty = await _wrapper.SpecialtyRepo.FindByCondition(sp => sp.Id == id)
                .FirstOrDefaultAsync();
            if (specialty == null)
                return new ResponseDto<SpecialtyDto>(MsgResponce.Specialty.NotFound, true);

            var saveSpecialty = form.Adapt(specialty);
            saveSpecialty.ModifierId = _userAccessor.UserId;
            saveSpecialty.ModifieAt = DateTime.Now;

            await _wrapper.SpecialtyRepo.Update(saveSpecialty);
            return new ResponseDto<SpecialtyDto>(saveSpecialty.Adapt<SpecialtyDto>());
        }

        public async Task<ResponseDto<bool>> Delete(int id)
        {
            var specialty = await _wrapper.SpecialtyRepo.FindItemByCondition(sp => sp.Id == id);
            if (specialty == null)
                return new ResponseDto<bool>(MsgResponce.Specialty.NotFound, true);

            specialty.DeleterId = _userAccessor.UserId;
            await _wrapper.SpecialtyRepo.Delete(id);
            await _wrapper.SaveAllAsync();
            return new ResponseDto<bool>(true);
        }
    }
}
