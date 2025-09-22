using DoctorsClinic.Core.Dtos;
using DoctorsClinic.Core.Dtos.Doctors;
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
    public class DoctorService : IDoctorService
    {
        private readonly IRepositoryWrapper _wrapper;
        private readonly IUserAccessorService _userAccessor;
        public DoctorService(IRepositoryWrapper wrapper, IUserAccessorService userAccessor)
        {
            _wrapper = wrapper;
            _userAccessor = userAccessor;
        }

        public async Task<ResponseDto<PaginationDto<DoctorDto>>> GetAll(PaginationQuery paginationQuery, DoctorFilterDto filter)
        {
            #region Apply Filter
            var query = _wrapper.DoctorRepo.GetAll()
                .Include(d => d.Specialty)
                .Include(d => d.User)
                .Where(d => string.IsNullOrEmpty(filter.FullName) || d.FullName.ToLower().Contains(filter.FullName.ToLower()))
                .Where(d => !filter.SpecialtyID.HasValue || d.SpecialtyID == filter.SpecialtyID)
                .Where(d => string.IsNullOrEmpty(filter.Phone) || d.Phone!.Contains(filter.Phone))
                .Where(d => string.IsNullOrEmpty(filter.Email) || d.Email!.ToLower().Contains(filter.Email.ToLower()));
            #endregion

            var data = await query
                .OrderByDescending(d => d.CreatedAt)
                .ApplyPagging(paginationQuery)
                .ProjectToType<DoctorDto>()
                .ToListAsync();

            var count = await query.CountAsync();
            var metadata = new PaginationMetadata(count, paginationQuery);

            return new ResponseDto<PaginationDto<DoctorDto>>(
                new PaginationDto<DoctorDto>(data, metadata));
        }

        public async Task<ResponseDto<IEnumerable<ListDto<int>>>> GetList()
        {
            var query = _wrapper.DoctorRepo.GetAll();

            return new ResponseDto<IEnumerable<ListDto<int>>>(
                await query.OrderBy(d => d.FullName)
                .Select(d => new ListDto<int>
                {
                    Id = d.Id,
                    Title = d.FullName
                }).ToListAsync()
            );
        }

        public async Task<ResponseDto<DoctorResponseDto>> GetById(int id)
        {
            var doctor = await _wrapper.DoctorRepo.FindByCondition(d => d.Id == id)
                .Include(d => d.Specialty)
                .Include(d => d.Appointments)
                .Include(d => d.MedicalRecords)
                .Include(d => d.Prescriptions)
                .FirstOrDefaultAsync();

            if (doctor == null)
                return new ResponseDto<DoctorResponseDto>(MsgResponce.Doctor.NotFound, true);

            return new ResponseDto<DoctorResponseDto>(doctor.Adapt<DoctorResponseDto>());
        }

        public async Task<ResponseDto<DoctorDto>> Add(CreateDoctorDto form)
        {
            var specialty = await _wrapper.SpecialtyRepo.FindItemByCondition(s => s.Id == form.SpecialtyID);
            if (specialty == null)
                return new ResponseDto<DoctorDto>(MsgResponce.Specialty.NotFound, true);

            var user = await _wrapper.UserRepo.FindItemByCondition(u => u.Id == form.UserID);
            if (user == null)
                return new ResponseDto<DoctorDto>(MsgResponce.User.NotFound, true);

            var doctor = form.Adapt<Doctor>();
            doctor.CreatorId = _userAccessor.UserId;
            doctor.CreatedAt = DateTime.Now;

            await _wrapper.DoctorRepo.Insert(doctor);
            await _wrapper.SaveAllAsync();

            doctor = await _wrapper.DoctorRepo.FindByCondition(d => d.Id == doctor.Id)
                .Include(d => d.Specialty)
                .Include(d => d.User)
                .FirstOrDefaultAsync();

            return new ResponseDto<DoctorDto>(doctor.Adapt<DoctorDto>());
        }

        public async Task<ResponseDto<DoctorDto>> Update(int id, UpdateDoctorDto form)
        {
            var doctor = await _wrapper.DoctorRepo.FindByCondition(d => d.Id == id)
                .Include(d => d.Specialty)
                .Include(d => d.User)
                .FirstOrDefaultAsync();
            if (doctor == null)
                return new ResponseDto<DoctorDto>(MsgResponce.Doctor.NotFound, true);

            var specialty = await _wrapper.SpecialtyRepo.FindItemByCondition(s => s.Id == form.SpecialtyID);
            if (specialty == null)
                return new ResponseDto<DoctorDto>(MsgResponce.Specialty.NotFound, true);

            var user = await _wrapper.UserRepo.FindItemByCondition(u => u.Id == form.UserID);
            if (user == null)
                return new ResponseDto<DoctorDto>(MsgResponce.User.NotFound, true);

            var saveDoctor = form.Adapt(doctor);
            saveDoctor.ModifierId = _userAccessor.UserId;
            saveDoctor.ModifieAt = DateTime.Now;

            await _wrapper.DoctorRepo.Update(saveDoctor);
            return new ResponseDto<DoctorDto>(saveDoctor.Adapt<DoctorDto>());
        }

        public async Task<ResponseDto<bool>> Delete(int id)
        {
            var doctor = await _wrapper.DoctorRepo.FindItemByCondition(d => d.Id == id);
            if (doctor == null)
                return new ResponseDto<bool>(MsgResponce.Doctor.NotFound, true);

            doctor.DeleterId = _userAccessor.UserId;
            await _wrapper.DoctorRepo.Delete(id);
            await _wrapper.SaveAllAsync();
            return new ResponseDto<bool>(true);
        }
    }
}
