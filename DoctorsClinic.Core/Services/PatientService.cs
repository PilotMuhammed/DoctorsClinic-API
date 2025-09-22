using DoctorsClinic.Core.Dtos;
using DoctorsClinic.Core.Dtos.Patients;
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
    public class PatientService : IPatientService
    {
        private readonly IRepositoryWrapper _wrapper;
        private readonly IUserAccessorService _userAccessor;
        public PatientService(IRepositoryWrapper wrapper, IUserAccessorService userAccessor)
        {
            _wrapper = wrapper;
            _userAccessor = userAccessor;
        }

        public async Task<ResponseDto<PaginationDto<PatientDto>>> GetAll(PaginationQuery paginationQuery, PatientFilterDto filter)
        {
            #region Apply Filter
            var query = _wrapper.PatientRepo.GetAll()
                .Where(pa => string.IsNullOrEmpty(filter.FullName) || pa.FullName.ToLower().Contains(filter.FullName.ToLower()))
                .Where(pa => !filter.Gender.HasValue || pa.Gender == filter.Gender)
                .Where(pa => !filter.DateOfBirth.HasValue || pa.DateOfBirth == filter.DateOfBirth)
                .Where(pa => string.IsNullOrEmpty(filter.Phone) || pa.Phone!.ToLower().Contains(filter.Phone.ToLower()))
                .Where(pa => string.IsNullOrEmpty(filter.Address) || pa.Address!.ToLower().Contains(filter.Address.ToLower()));
            #endregion

            var data = await query
                .OrderByDescending(pa => pa.CreatedAt)
                .ApplyPagging(paginationQuery)
                .ProjectToType<PatientDto>()
                .ToListAsync();

            var count = await query.CountAsync();
            var metadata = new PaginationMetadata(count, paginationQuery);

            return new ResponseDto<PaginationDto<PatientDto>>(
                new PaginationDto<PatientDto>(data, metadata));
        }

        public async Task<ResponseDto<IEnumerable<ListDto<int>>>> GetList()
        {
            var query = _wrapper.PatientRepo.GetAll();

            return new ResponseDto<IEnumerable<ListDto<int>>>(
                await query.OrderBy(pa => pa.FullName)
                .Select(pa => new ListDto<int>
                {
                    Id = pa.Id,
                    Title = pa.FullName
                }).ToListAsync()
            );
        }

        public async Task<ResponseDto<PatientResponseDto>> GetById(int id)
        {
            var patient = await _wrapper.PatientRepo.FindByCondition(pa => pa.Id == id)
                .Include(pa => pa.Appointments)
                .Include(pa => pa.MedicalRecords)
                .Include(pa => pa.Prescriptions)
                .Include(pa => pa.Invoices)
                .FirstOrDefaultAsync();

            if (patient == null)
                return new ResponseDto<PatientResponseDto>(MsgResponce.Patient.NotFound, true);

            return new ResponseDto<PatientResponseDto>(patient.Adapt<PatientResponseDto>());
        }

        public async Task<ResponseDto<PatientDto>> Add(CreatePatientDto form)
        {
            var patient = form.Adapt<Patient>();
            patient.CreatorId = _userAccessor.UserId;
            patient.CreatedAt = DateTime.Now;

            await _wrapper.PatientRepo.Insert(patient);
            await _wrapper.SaveAllAsync();

            patient = await _wrapper.PatientRepo.FindByCondition(pa => pa.Id == patient.Id)
                .FirstOrDefaultAsync();

            return new ResponseDto<PatientDto>(patient.Adapt<PatientDto>());
        }

        public async Task<ResponseDto<PatientDto>> Update(int id, UpdatePatientDto form)
        {
            var patient = await _wrapper.PatientRepo.FindByCondition(pa => pa.Id == id)
                .FirstOrDefaultAsync();
            if (patient == null)
                return new ResponseDto<PatientDto>(MsgResponce.Patient.NotFound, true);

            var savePatient = form.Adapt(patient);
            savePatient.ModifierId = _userAccessor.UserId;
            savePatient.ModifieAt = DateTime.Now;

            await _wrapper.PatientRepo.Update(savePatient);
            return new ResponseDto<PatientDto>(savePatient.Adapt<PatientDto>());
        }

        public async Task<ResponseDto<bool>> Delete(int id)
        {
            var patient = await _wrapper.PatientRepo.FindItemByCondition(pa => pa.Id == id);
            if (patient == null)
                return new ResponseDto<bool>(MsgResponce.Patient.NotFound, true);

            patient.DeleterId = _userAccessor.UserId;
            await _wrapper.PatientRepo.Delete(id);
            await _wrapper.SaveAllAsync();
            return new ResponseDto<bool>(true);
        }
    }
}
