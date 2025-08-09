using DoctorsClinic.Domain.Entities;
using DoctorsClinic.Core.Dtos.Appointments;
using DoctorsClinic.Domain.Enums; 
using Mapster;

namespace DoctorsClinic.Core.Mapster
{
    public class AppointmentMap
    {
        public static void Configure()
        {
            
            TypeAdapterConfig<Appointment, AppointmentDto>.NewConfig()
                .Map(d => d.PatientName, s => s.Patient != null ? s.Patient.FullName : null)
                .Map(d => d.DoctorName, s => s.Doctor != null ? s.Doctor.FullName : null)
                .Map(d => d.Status, s => s.Status.ToString());

            TypeAdapterConfig<AppointmentDto, Appointment>.NewConfig()
                .Map(d => d.Status, s => ParseStatus(s.Status))
                .Map(d => d.Notes, s => s.Notes ?? string.Empty)
                .Ignore(d => d.Patient)
                .Ignore(d => d.Doctor)
                .Ignore(d => d.Prescriptions)
                .Ignore(d => d.Invoice);

            TypeAdapterConfig<CreateAppointmentDto, Appointment>.NewConfig()
                .Map(d => d.Status, s => ParseStatus(s.Status))
                .Map(d => d.Notes, s => s.Notes ?? string.Empty)
                .Ignore(d => d.AppointmentID)
                .Ignore(d => d.Patient)
                .Ignore(d => d.Doctor)
                .Ignore(d => d.Prescriptions)
                .Ignore(d => d.Invoice);

            TypeAdapterConfig<UpdateAppointmentDto, Appointment>.NewConfig()
                .IgnoreIf((s, _) => s.PatientID == null, d => d.PatientID)
                .IgnoreIf((s, _) => s.DoctorID == null, d => d.DoctorID)
                .IgnoreIf((s, _) => s.AppointmentDate == null, d => d.AppointmentDate)
                .IgnoreIf((s, _) => string.IsNullOrWhiteSpace(s.Status), d => d.Status)
                .IgnoreIf((s, _) => s.Notes == null, d => d.Notes)
                .Map(d => d.Status, s => s.Status == null ? default : ParseStatus(s.Status))
                .Map(d => d.Notes, s => s.Notes ?? string.Empty)
                .Ignore(d => d.Patient)
                .Ignore(d => d.Doctor)
                .Ignore(d => d.Prescriptions)
                .Ignore(d => d.Invoice);

            TypeAdapterConfig<Appointment, AppointmentResponseDto>.NewConfig()
                .Map(d => d.Appointment, s => s.Adapt<AppointmentDto>()!) 
                .Ignore(d => d.Patient)
                .Ignore(d => d.Doctor)
                .Ignore(d => d.Prescriptions)
                .Ignore(d => d.Invoice);
        }


        private static AppointmentStatus ParseStatus(string? status)
        {
            return Enum.TryParse<AppointmentStatus>(status, true, out var value) ? value : default;
        }
    }
}
