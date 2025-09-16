using DoctorsClinic.Domain.Entities;
using DoctorsClinic.Core.Dtos.Appointments;
using Mapster;
using DoctorsClinic.Core.Dtos.Patients;
using DoctorsClinic.Core.Dtos.Doctors;
using DoctorsClinic.Core.Dtos.Prescriptions;
using DoctorsClinic.Core.Dtos.Invoices;

namespace DoctorsClinic.Core.Mapster
{
    public class AppointmentMap
    {
        public static void Configure()
        {
            TypeAdapterConfig<CreateAppointmentDto, Appointment>.NewConfig()
                .Ignore(dest => dest.Prescriptions!)
                .Ignore(dest => dest.Invoice!);

            TypeAdapterConfig<Appointment, AppointmentResponseDto>.NewConfig()
                .Map(dest => dest.Appointment, src => src.Adapt<AppointmentDto>())
                .Map(dest => dest.Patient, src => src.Patient.Adapt<PatientDto>())
                .Map(dest => dest.Doctor, src => src.Doctor.Adapt<DoctorDto>())
                .Map(dest => dest.Prescriptions, src => src.Prescriptions.Adapt<List<PrescriptionDto>>())
                .Map(dest => dest.Invoice, src => src.Invoice.Adapt<InvoiceDto>());

            TypeAdapterConfig<Appointment, AppointmentDto>.NewConfig();

            TypeAdapterConfig<UpdateAppointmentDto, Appointment>.NewConfig()
                .IgnoreIf((src, dest) => src.PatientID == null, dest => dest.PatientID)
                .IgnoreIf((src, dest) => src.DoctorID == null, dest => dest.DoctorID)
                .IgnoreIf((src, dest) => src.AppointmentDate == null, dest => dest.AppointmentDate)
                .IgnoreIf((src, dest) => src.Status == null, dest => dest.Status)
                .IgnoreIf((src, dest) => string.IsNullOrWhiteSpace(src.Notes), dest => dest.Notes!)
                .Ignore(dest => dest.Prescriptions!)
                .Ignore(dest => dest.Invoice!);
        }
    }
}
