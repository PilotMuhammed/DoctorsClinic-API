using DoctorsClinic.Core.Dtos;
using DoctorsClinic.Core.Dtos.Prescriptions;
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
    public class PrescriptionService : IPrescriptionService
    {
        private readonly IRepositoryWrapper _wrapper;
        private readonly IUserAccessorService _userAccessor;
        public PrescriptionService(IRepositoryWrapper wrapper, IUserAccessorService userAccessor)
        {
            _wrapper = wrapper;
            _userAccessor = userAccessor;
        }

        public async Task<ResponseDto<PaginationDto<PrescriptionDto>>> GetAll(PaginationQuery paginationQuery, PrescriptionFilterDto filter)
        {
            #region Apply Filter
            var query = _wrapper.PrescriptionRepo.GetAll()
                .Include(pr => pr.Appointment)
                .Include(pr => pr.Doctor)
                .Include(pr => pr.Patient)
                .Where(pr => !filter.AppointmentID.HasValue || pr.AppointmentID == filter.AppointmentID)
                .Where(pr => !filter.DoctorID.HasValue || pr.DoctorID == filter.DoctorID)
                .Where(pr => !filter.PatientID.HasValue || pr.PatientID == filter.PatientID)
                .Where(pr => string.IsNullOrEmpty(filter.Notes) || pr.Notes!.ToLower().Contains(filter.Notes.ToLower()));
            #endregion

            var data = await query
                .OrderByDescending(pr => pr.CreatedAt)
                .ApplyPagging(paginationQuery)
                .ProjectToType<PrescriptionDto>()
                .ToListAsync();

            var count = await query.CountAsync();
            var metadata = new PaginationMetadata(count, paginationQuery);

            return new ResponseDto<PaginationDto<PrescriptionDto>>(
                new PaginationDto<PrescriptionDto>(data, metadata));
        }

        public async Task<ResponseDto<PrescriptionResponseDto>> GetById(int id)
        {
            var prescription = await _wrapper.PrescriptionRepo.FindByCondition(pr => pr.Id == id)
                .Include(pr => pr.Appointment)
                .Include(pr => pr.Doctor)
                .Include(pr => pr.Patient)
                .Include(pr => pr.PrescriptionMedicines)
                .FirstOrDefaultAsync();

            if (prescription == null)
                return new ResponseDto<PrescriptionResponseDto>(MsgResponce.Prescription.NotFound, true);

            return new ResponseDto<PrescriptionResponseDto>(prescription.Adapt<PrescriptionResponseDto>());
        }

        public async Task<ResponseDto<PrescriptionDto>> Add(CreatePrescriptionDto form)
        {
            var appointment = await _wrapper.AppointmentRepo.FindItemByCondition(ap => ap.Id == form.AppointmentID);
            if (appointment == null)
                return new ResponseDto<PrescriptionDto>(MsgResponce.Appointment.NotFound, true);

            var doctor = await _wrapper.DoctorRepo.FindItemByCondition(d => d.Id == form.DoctorID);
            if (doctor == null)
                return new ResponseDto<PrescriptionDto>(MsgResponce.Doctor.NotFound, true);

            var patient = await _wrapper.PatientRepo.FindItemByCondition(pa => pa.Id == form.DoctorID);
            if (patient == null)
                return new ResponseDto<PrescriptionDto>(MsgResponce.Patient.NotFound, true);

            var prescription = form.Adapt<Prescription>();
            prescription.CreatorId = _userAccessor.UserId;
            prescription.CreatedAt = DateTime.Now;

            await _wrapper.PrescriptionRepo.Insert(prescription);
            await _wrapper.SaveAllAsync();

            prescription = await _wrapper.PrescriptionRepo.FindByCondition(pr => pr.Id == prescription.Id)
                .Include(pr => pr.Appointment)
                .Include(pr => pr.Doctor)
                .Include(pr => pr.Patient)
                .FirstOrDefaultAsync();

            return new ResponseDto<PrescriptionDto>(prescription.Adapt<PrescriptionDto>());
        }

        public async Task<ResponseDto<PrescriptionDto>> Update(int id, UpdatePrescriptionDto form)
        {
            var prescription = await _wrapper.PrescriptionRepo.FindByCondition(pr => pr.Id == id)
                .Include(pr => pr.Appointment)
                .Include(pr => pr.Doctor)
                .Include(pr => pr.Patient)
                .FirstOrDefaultAsync();
            if (prescription == null)
                return new ResponseDto<PrescriptionDto>(MsgResponce.Prescription.NotFound, true);

            var appointment = await _wrapper.AppointmentRepo.FindItemByCondition(ap => ap.Id == form.AppointmentID);
            if (appointment == null)
                return new ResponseDto<PrescriptionDto>(MsgResponce.Appointment.NotFound, true);

            var doctor = await _wrapper.DoctorRepo.FindItemByCondition(d => d.Id == form.DoctorID);
            if (doctor == null)
                return new ResponseDto<PrescriptionDto>(MsgResponce.Doctor.NotFound, true);

            var patient = await _wrapper.PatientRepo.FindItemByCondition(pa => pa.Id == form.DoctorID);
            if (patient == null)
                return new ResponseDto<PrescriptionDto>(MsgResponce.Patient.NotFound, true);

            var savePrescription = form.Adapt(prescription);
            savePrescription.ModifierId = _userAccessor.UserId;
            savePrescription.ModifieAt = DateTime.Now;

            await _wrapper.PrescriptionRepo.Update(savePrescription);
            return new ResponseDto<PrescriptionDto>(savePrescription.Adapt<PrescriptionDto>());
        }

        public async Task<ResponseDto<bool>> Delete(int id)
        {
            var prescription = await _wrapper.PrescriptionRepo.FindItemByCondition(pr => pr.Id == id);
            if (prescription == null)
                return new ResponseDto<bool>(MsgResponce.Prescription.NotFound, true);

            prescription.DeleterId = _userAccessor.UserId;
            await _wrapper.PrescriptionRepo.Delete(id);
            await _wrapper.SaveAllAsync();
            return new ResponseDto<bool>(true);
        }
    }
}
