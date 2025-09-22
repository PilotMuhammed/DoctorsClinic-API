using DoctorsClinic.Core.Dtos;
using DoctorsClinic.Core.Dtos.Invoices;
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
    public class InvoiceService : IInvoiceService
    {
        private readonly IRepositoryWrapper _wrapper;
        private readonly IUserAccessorService _userAccessor;
        public InvoiceService(IRepositoryWrapper wrapper, IUserAccessorService userAccessor)
        {
            _wrapper = wrapper;
            _userAccessor = userAccessor;
        }

        public async Task<ResponseDto<PaginationDto<InvoiceDto>>> GetAll(PaginationQuery paginationQuery, InvoiceFilterDto filter)
        {
            #region Apply Filter
            var query = _wrapper.InvoiceRepo.GetAll()
                .Include(i => i.Patient)
                .Include(i => i.Appointment)
                .Where(i => !filter.PatientID.HasValue || i.PatientID == filter.PatientID)
                .Where(i => !filter.AppointmentID.HasValue || i.AppointmentID == filter.AppointmentID)
                .Where(i => !filter.Status.HasValue || i.Status == filter.Status)
                .Where(i => !filter.Date.HasValue || i.Date == filter.Date)
                .Where(i => !filter.CreatedAt.HasValue || i.CreatedAt == filter.CreatedAt);
            #endregion

            var data = await query
                .OrderByDescending(i => i.CreatedAt)
                .ApplyPagging(paginationQuery)
                .ProjectToType<InvoiceDto>()
                .ToListAsync();

            var count = await query.CountAsync();
            var metadata = new PaginationMetadata(count, paginationQuery);

            return new ResponseDto<PaginationDto<InvoiceDto>>(
                new PaginationDto<InvoiceDto>(data, metadata));
        }

        public async Task<ResponseDto<InvoiceResponseDto>> GetById(int id)
        {
            var invoice = await _wrapper.InvoiceRepo.FindByCondition(i => i.Id == id)
                .Include(i => i.Patient)
                .Include(i => i.Appointment)
                .Include(i => i.Payments)
                .FirstOrDefaultAsync();

            if (invoice == null)
                return new ResponseDto<InvoiceResponseDto>(MsgResponce.Invoice.NotFound, true);

            return new ResponseDto<InvoiceResponseDto>(invoice.Adapt<InvoiceResponseDto>());
        }

        public async Task<ResponseDto<InvoiceDto>> Add(CreateInvoiceDto form)
        {
            var patient = await _wrapper.PatientRepo.FindItemByCondition(p => p.Id == form.PatientID);
            if (patient == null)
                return new ResponseDto<InvoiceDto>(MsgResponce.Patient.NotFound, true);

            var appointment = await _wrapper.AppointmentRepo.FindItemByCondition(a => a.Id == form.AppointmentID);
            if (appointment == null)
                return new ResponseDto<InvoiceDto>(MsgResponce.Appointment.NotFound, true);

            if(form.TotalAmount <= 0)
                return new ResponseDto<InvoiceDto>(MsgResponce.AlarmAmounts.WrongEntry, true);

            var invoice = form.Adapt<Invoice>();
            invoice.CreatorId = _userAccessor.UserId;
            invoice.CreatedAt = DateTime.Now;

            await _wrapper.InvoiceRepo.Insert(invoice);
            await _wrapper.SaveAllAsync();

            invoice = await _wrapper.InvoiceRepo.FindByCondition(i => i.Id == invoice.Id)
                .Include(i => i.Patient)
                .Include(i => i.Appointment)
                .FirstOrDefaultAsync();

            return new ResponseDto<InvoiceDto>(invoice.Adapt<InvoiceDto>());
        }

        public async Task<ResponseDto<InvoiceDto>> Update(int id, UpdateInvoiceDto form)
        {
            var invoice = await _wrapper.InvoiceRepo.FindByCondition(i => i.Id == id)
                .Include(i => i.Patient)
                .Include(i => i.Appointment)
                .FirstOrDefaultAsync();
            if (invoice == null)
                return new ResponseDto<InvoiceDto>(MsgResponce.Invoice.NotFound, true);

            var patient = await _wrapper.PatientRepo.FindItemByCondition(p => p.Id == form.PatientID);
            if (patient == null)
                return new ResponseDto<InvoiceDto>(MsgResponce.Patient.NotFound, true);

            var appointment = await _wrapper.AppointmentRepo.FindItemByCondition(a => a.Id == form.AppointmentID);
            if (appointment == null)
                return new ResponseDto<InvoiceDto>(MsgResponce.Appointment.NotFound, true);

            if (form.TotalAmount <= 0)
                return new ResponseDto<InvoiceDto>(MsgResponce.AlarmAmounts.WrongEntry, true);

            var saveInvoice = form.Adapt(invoice);
            saveInvoice.ModifierId = _userAccessor.UserId;
            saveInvoice.ModifieAt = DateTime.Now;

            await _wrapper.InvoiceRepo.Update(saveInvoice);
            return new ResponseDto<InvoiceDto>(saveInvoice.Adapt<InvoiceDto>());
        }

        public async Task<ResponseDto<bool>> Delete(int id)
        {
            var invoice = await _wrapper.InvoiceRepo.FindItemByCondition(i => i.Id == id);
            if (invoice == null)
                return new ResponseDto<bool>(MsgResponce.Invoice.NotFound, true);

            invoice.DeleterId = _userAccessor.UserId;
            await _wrapper.InvoiceRepo.Delete(id);
            await _wrapper.SaveAllAsync();
            return new ResponseDto<bool>(true);
        }
    }
}
