using DoctorsClinic.Core.Dtos;
using DoctorsClinic.Core.Dtos.Invoices;
using DoctorsClinic.Core.Extensions;
using DoctorsClinic.Core.Helper;
using DoctorsClinic.Core.IServices;
using DoctorsClinic.Infrastructure.IRepositories;
using Mapster;
using Microsoft.EntityFrameworkCore;
using DoctorsClinic.Domain.Enums;

namespace DoctorsClinic.Core.Services
{
    public class InvoiceService : IInvoiceService
    {
        private readonly IRepositoryWrapper _repo;

        public InvoiceService(IRepositoryWrapper repo)
        {
            _repo = repo;
        }

        // GetAllAsync 
        public async Task<ResponseDto<PaginationDto<InvoiceDto>>> GetAllAsync(
            PaginationQuery pagination,
            InvoiceFilterDto filter,
            CancellationToken ct = default)
        {
            if (pagination == null)
                return new ResponseDto<PaginationDto<InvoiceDto>>(MsgResponce.Failed, true);

            var query = _repo.Invoices.GetAll(include: q =>
                q.Include(i => i.Patient!)
                 .Include(i => i.Appointment!)
                 .Include(i => i.Payments!),
                track: false
            );

            if (filter != null)
            {
                if (filter.PatientID.HasValue)
                    query = query.Where(i => i.PatientID == filter.PatientID.Value);
                if (filter.Status != null)
                    query = query.Where(i => i.Status.ToString() == filter.Status);
                if (filter.DateFrom.HasValue)
                    query = query.Where(i => i.Date >= filter.DateFrom.Value);
                if (filter.DateTo.HasValue)
                    query = query.Where(i => i.Date <= filter.DateTo.Value);
            }

            var totalCount = await query.CountAsync(ct);
            var data = await query
                .ApplyPagging(pagination)
                .ProjectToType<InvoiceDto>()
                .ToListAsync(ct);

            var meta = new PaginationMetadata(totalCount, pagination);
            var paginated = new PaginationDto<InvoiceDto>(data, meta);

            if (!data.Any())
                return new ResponseDto<PaginationDto<InvoiceDto>>(MsgResponce.Invoice.NotFound, true);

            return new ResponseDto<PaginationDto<InvoiceDto>>(paginated);
        }

        // GetByIdAsync
        public async Task<ResponseDto<InvoiceResponseDto>> GetByIdAsync(int id, CancellationToken ct = default)
        {
            if (id <= 0)
                return new ResponseDto<InvoiceResponseDto>(MsgResponce.Invoice.NotFound, true);

            var invoice = await _repo.Invoices.GetByIdAsync(id, include: q =>
                q.Include(i => i.Patient!)
                 .Include(i => i.Appointment!)
                 .Include(i => i.Payments!), track: false);

            if (invoice == null)
                return new ResponseDto<InvoiceResponseDto>(MsgResponce.Invoice.NotFound, true);

            var dto = invoice.Adapt<InvoiceResponseDto>();
            return new ResponseDto<InvoiceResponseDto>(dto);
        }

        // GetByPatientAsync 
        public async Task<ResponseDto<List<InvoiceDto>>> GetByPatientAsync(int patientId, CancellationToken ct = default)
        {
            if (patientId <= 0)
                return new ResponseDto<List<InvoiceDto>>(MsgResponce.Patient.NotFound, true);

            var invoices = await _repo.Invoices
                .FindByCondition(i => i.PatientID == patientId, include: q =>
                    q.Include(i => i.Appointment!)
                     .Include(i => i.Payments!), track: false)
                .ProjectToType<InvoiceDto>()
                .ToListAsync(ct);

            if (!invoices.Any())
                return new ResponseDto<List<InvoiceDto>>(MsgResponce.Invoice.NotFound, true);

            return new ResponseDto<List<InvoiceDto>>(invoices);
        }

        // CreateAsync 
        public async Task<ResponseDto<InvoiceDto>> CreateAsync(CreateInvoiceDto dto, CancellationToken ct = default)
        {
            if (dto == null)
                return new ResponseDto<InvoiceDto>(MsgResponce.Failed, true);

            var patient = await _repo.Patients.GetByIdAsync(dto.PatientID, track: false);
            if (patient == null)
                return new ResponseDto<InvoiceDto>(MsgResponce.Patient.NotFound, true);

            var appointment = await _repo.Appointments.GetByIdAsync(dto.AppointmentID, track: false);
            if (appointment == null)
                return new ResponseDto<InvoiceDto>(MsgResponce.Appointment.NotFound, true);

            var invoice = dto.Adapt<Domain.Entities.Invoice>();
            await _repo.Invoices.AddAsync(invoice);
            await _repo.SaveChangesAsync();

            var resultDto = invoice.Adapt<InvoiceDto>();
            return new ResponseDto<InvoiceDto>(resultDto);
        }

        // GenerateForAppointmentAsync 
        public async Task<ResponseDto<InvoiceDto>> GenerateForAppointmentAsync(int appointmentId, CancellationToken ct = default)
        {
            if (appointmentId <= 0)
                return new ResponseDto<InvoiceDto>(MsgResponce.Appointment.NotFound, true);

            var appointment = await _repo.Appointments.GetByIdAsync(appointmentId, include: q =>
                q.Include(a => a.Patient!), track: false);

            if (appointment == null)
                return new ResponseDto<InvoiceDto>(MsgResponce.Appointment.NotFound, true);


            var invoice = new Domain.Entities.Invoice
            {
                PatientID = appointment.PatientID,
                AppointmentID = appointment.AppointmentID,
                Date = DateTime.Now,
                Status = Domain.Enums.InvoiceStatus.Unpaid,
                TotalAmount = 0 
            };

            await _repo.Invoices.AddAsync(invoice);
            await _repo.SaveChangesAsync();

            var dto = invoice.Adapt<InvoiceDto>();
            return new ResponseDto<InvoiceDto>(dto);
        }

        // UpdateAsync
        public async Task<ResponseDto<InvoiceDto>> UpdateAsync(int id, UpdateInvoiceDto dto, CancellationToken ct = default)
        {
            if (id <= 0 || dto == null)
                return new ResponseDto<InvoiceDto>(MsgResponce.Failed, true);

            var invoice = await _repo.Invoices.GetByIdAsync(id, track: true);
            if (invoice == null)
                return new ResponseDto<InvoiceDto>(MsgResponce.Invoice.NotFound, true);

            // Update
            if (dto.TotalAmount.HasValue && dto.TotalAmount.Value > 0)
                invoice.TotalAmount = dto.TotalAmount.Value;

            if (!string.IsNullOrWhiteSpace(dto.Status))
            {
                if (Enum.TryParse<Domain.Enums.InvoiceStatus>(dto.Status, out var statusEnum))
                    invoice.Status = statusEnum;
                else
                    return new ResponseDto<InvoiceDto>("Invalid invoice status.", true);
            }

            if (dto.Date.HasValue)
                invoice.Date = dto.Date.Value;

            _repo.Invoices.Update(invoice);
            await _repo.SaveChangesAsync();

            var resultDto = invoice.Adapt<InvoiceDto>();
            return new ResponseDto<InvoiceDto>(resultDto);
        }

        // MarkAsPaidAsync 
        public async Task<ResponseDto<InvoiceDto>> MarkAsPaidAsync(int invoiceId, CancellationToken ct = default)
        {
            if (invoiceId <= 0)
                return new ResponseDto<InvoiceDto>(MsgResponce.Invoice.NotFound, true);

            var invoice = await _repo.Invoices.GetByIdAsync(invoiceId, track: true);
            if (invoice == null)
                return new ResponseDto<InvoiceDto>(MsgResponce.Invoice.NotFound, true);

            invoice.Status = Domain.Enums.InvoiceStatus.Paid;

            _repo.Invoices.Update(invoice);
            await _repo.SaveChangesAsync();

            var dto = invoice.Adapt<InvoiceDto>();
            return new ResponseDto<InvoiceDto>(dto);
        }

        // DeleteAsync 
        public async Task<ResponseDto<bool>> DeleteAsync(int id, CancellationToken ct = default)
        {
            if (id <= 0)
                return new ResponseDto<bool>(MsgResponce.Invoice.NotFound, true);

            var invoice = await _repo.Invoices.GetByIdAsync(id, track: true);
            if (invoice == null)
                return new ResponseDto<bool>(MsgResponce.Invoice.NotFound, true);

            _repo.Invoices.Remove(invoice);
            await _repo.SaveChangesAsync();

            return new ResponseDto<bool>(true);
        }
    }
}
