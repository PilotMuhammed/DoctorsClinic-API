using DoctorsClinic.Core.Dtos;
using DoctorsClinic.Core.Dtos.MedicalRecords;
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
    public class MedicalRecordService : IMedicalRecordService
    {
        private readonly IRepositoryWrapper _wrapper;
        private readonly IUserAccessorService _userAccessor;
        public MedicalRecordService(IRepositoryWrapper wrapper, IUserAccessorService userAccessor)
        {
            _wrapper = wrapper;
            _userAccessor = userAccessor;
        }

        public async Task<ResponseDto<PaginationDto<MedicalRecordDto>>> GetAll(PaginationQuery paginationQuery, MedicalRecordFilterDto filter)
        {
            #region Apply Filter
            var query = _wrapper.MedicalRecordRepo.GetAll()
                .Include(m => m.Patient)
                .Include(m => m.Doctor)
                .Where(m => !filter.PatientID.HasValue || m.PatientID == filter.PatientID)
                .Where(m => !filter.DoctorID.HasValue || m.DoctorID == filter.DoctorID)
                .Where(m => string.IsNullOrEmpty(filter.Diagnosis) || m.Diagnosis!.ToLower().Contains(filter.Diagnosis.ToLower()))
                .Where(m => string.IsNullOrEmpty(filter.Notes) || m.Notes!.ToLower().Contains(filter.Notes.ToLower()));
            #endregion

            var data = await query
                .OrderByDescending(m => m.CreatedAt)
                .ApplyPagging(paginationQuery)
                .ProjectToType<MedicalRecordDto>()
                .ToListAsync();

            var count = await query.CountAsync();
            var metadata = new PaginationMetadata(count, paginationQuery);

            return new ResponseDto<PaginationDto<MedicalRecordDto>>(
                new PaginationDto<MedicalRecordDto>(data, metadata));
        }

        public async Task<ResponseDto<MedicalRecordResponseDto>> GetById(int id)
        {
            var record = await _wrapper.MedicalRecordRepo.FindByCondition(m => m.Id == id)
                .Include(m => m.Patient)
                .Include(m => m.Doctor)
                .FirstOrDefaultAsync();

            if (record == null)
                return new ResponseDto<MedicalRecordResponseDto>(MsgResponce.MedicalRecord.NotFound, true);

            return new ResponseDto<MedicalRecordResponseDto>(record.Adapt<MedicalRecordResponseDto>());
        }

        public async Task<ResponseDto<MedicalRecordDto>> Add(CreateMedicalRecordDto form)
        {
            var patient = await _wrapper.PatientRepo.FindItemByCondition(p => p.Id == form.PatientID);
            if (patient == null)
                return new ResponseDto<MedicalRecordDto>(MsgResponce.Patient.NotFound, true);

            var doctor = await _wrapper.DoctorRepo.FindItemByCondition(d => d.Id == form.DoctorID);
            if (doctor == null)
                return new ResponseDto<MedicalRecordDto>(MsgResponce.Doctor.NotFound, true);

            var record = form.Adapt<MedicalRecord>();
            record.CreatorId = _userAccessor.UserId;
            record.CreatedAt = DateTime.Now;

            await _wrapper.MedicalRecordRepo.Insert(record);
            await _wrapper.SaveAllAsync();

            record = await _wrapper.MedicalRecordRepo.FindByCondition(m => m.Id == record.Id)
                .Include(m => m.Patient)
                .Include(m => m.Doctor)
                .FirstOrDefaultAsync();

            return new ResponseDto<MedicalRecordDto>(record.Adapt<MedicalRecordDto>());
        }

        public async Task<ResponseDto<MedicalRecordDto>> Update(int id, UpdateMedicalRecordDto form)
        {
            var record = await _wrapper.MedicalRecordRepo.FindByCondition(m => m.Id == id)
                .Include(m => m.Patient)
                .Include(m => m.Doctor)
                .FirstOrDefaultAsync();
            if (record == null)
                return new ResponseDto<MedicalRecordDto>(MsgResponce.MedicalRecord.NotFound, true);

            var patient = await _wrapper.PatientRepo.FindItemByCondition(p => p.Id == form.PatientID);
            if (patient == null)
                return new ResponseDto<MedicalRecordDto>(MsgResponce.Patient.NotFound, true);

            var doctor = await _wrapper.DoctorRepo.FindItemByCondition(d => d.Id == form.DoctorID);
            if (doctor == null)
                return new ResponseDto<MedicalRecordDto>(MsgResponce.Doctor.NotFound, true);

            var saveRecord = form.Adapt(record);
            saveRecord.ModifierId = _userAccessor.UserId;
            saveRecord.ModifieAt = DateTime.Now;

            await _wrapper.MedicalRecordRepo.Update(saveRecord);
            return new ResponseDto<MedicalRecordDto>(saveRecord.Adapt<MedicalRecordDto>());
        }

        public async Task<ResponseDto<bool>> Delete(int id)
        {
            var record = await _wrapper.MedicalRecordRepo.FindItemByCondition(m => m.Id == id);
            if (record == null)
                return new ResponseDto<bool>(MsgResponce.MedicalRecord.NotFound, true);

            record.DeleterId = _userAccessor.UserId;
            await _wrapper.MedicalRecordRepo.Delete(id);
            await _wrapper.SaveAllAsync();
            return new ResponseDto<bool>(true);
        }
    }
}
