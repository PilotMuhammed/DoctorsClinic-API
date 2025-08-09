using DoctorsClinic.Core.Dtos.Appointments;
using DoctorsClinic.Core.Dtos.Invoices;
using DoctorsClinic.Core.Dtos.Patients;
using DoctorsClinic.Core.Dtos.Payments;
using DoctorsClinic.Domain.Entities;
using DoctorsClinic.Domain.Enums; 
using Mapster;

namespace DoctorsClinic.Core.Mapster
{
    public class InvoiceMap
    {
        public static void Configure()
        {

            TypeAdapterConfig<Invoice, InvoiceDto>.NewConfig()
                .Map(d => d.Status, s => s.Status.ToString())
                .Map(d => d.PatientName, s => s.Patient != null ? s.Patient.FullName : null);

            TypeAdapterConfig<InvoiceDto, Invoice>.NewConfig()
                .Map(d => d.Status, s => ParseStatus(s.Status))
                .Ignore(d => d.Patient)
                .Ignore(d => d.Appointment)
                .Ignore(d => d.Payments);

            TypeAdapterConfig<CreateInvoiceDto, Invoice>.NewConfig()
                .Map(d => d.Status, s => ParseStatus(s.Status))
                .Ignore(d => d.InvoiceID)
                .Ignore(d => d.Patient)
                .Ignore(d => d.Appointment)
                .Ignore(d => d.Payments);

            TypeAdapterConfig<UpdateInvoiceDto, Invoice>.NewConfig()
                .IgnoreIf((s, _) => s.PatientID == null, d => d.PatientID)
                .IgnoreIf((s, _) => s.AppointmentID == null, d => d.AppointmentID)
                .IgnoreIf((s, _) => s.TotalAmount == null, d => d.TotalAmount)
                .IgnoreIf((s, _) => string.IsNullOrWhiteSpace(s.Status), d => d.Status)
                .IgnoreIf((s, _) => s.Date == null, d => d.Date)
                .Map(d => d.Status, s => s.Status == null ? default : ParseStatus(s.Status))
                .Ignore(d => d.Patient)
                .Ignore(d => d.Appointment)
                .Ignore(d => d.Payments);

            TypeAdapterConfig<Invoice, InvoiceResponseDto>.NewConfig()
                .Map(d => d.Invoice, s => s.Adapt<InvoiceDto>()!) 
                .Map(d => d.Patient, _ => (PatientDto?)null)
                .Map(d => d.Appointment, _ => (AppointmentDto?)null)
                .Map(d => d.Payments, _ => (List<PaymentDto>?)null);
        }

        private static InvoiceStatus ParseStatus(string? status)
        {
            return Enum.TryParse<InvoiceStatus>(status, true, out var value) ? value : default;
        }
    }
}
