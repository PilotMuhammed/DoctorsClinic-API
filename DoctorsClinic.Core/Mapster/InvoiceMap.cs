using DoctorsClinic.Core.Dtos.Appointments;
using DoctorsClinic.Core.Dtos.Invoices;
using DoctorsClinic.Core.Dtos.Patients;
using DoctorsClinic.Core.Dtos.Payments;
using DoctorsClinic.Domain.Entities;
using Mapster;

namespace DoctorsClinic.Core.Mapster
{
    public class InvoiceMap
    {
        public static void Configure()
        {
            TypeAdapterConfig<CreateInvoiceDto, Invoice>.NewConfig()
                .Ignore(dest => dest.Payments!);

            TypeAdapterConfig<Invoice, InvoiceResponseDto>.NewConfig()
                .Map(dest => dest.Invoice, src => src.Adapt<InvoiceDto>())
                .Map(dest => dest.Patient, src => src.Patient.Adapt<PatientDto>())
                .Map(dest => dest.Appointment, src => src.Appointment.Adapt<AppointmentDto>())
                .Map(dest => dest.Payments, src => src.Payments.Adapt<List<PaymentDto>>());

            TypeAdapterConfig<Invoice, InvoiceDto>.NewConfig();

            TypeAdapterConfig<UpdateInvoiceDto, Invoice>.NewConfig()
                .IgnoreIf((src, dest) => src.PatientID == null, dest => dest.PatientID)
                .IgnoreIf((src, dest) => src.AppointmentID == null, dest => dest.AppointmentID)
                .IgnoreIf((src, dest) => src.TotalAmount == null, dest => dest.TotalAmount)
                .IgnoreIf((src, dest) => src.Status == null, dest => dest.Status)
                .IgnoreIf((src, dest) => src.Date == null, dest => dest.Date)
                .Ignore(dest => dest.Payments!);
        }
    }
}
