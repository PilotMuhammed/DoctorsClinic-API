using DoctorsClinic.Domain.Entities;
using DoctorsClinic.Core.Dtos.Prescriptions;
using DoctorsClinic.Core.Dtos.Appointments;
using DoctorsClinic.Core.Dtos.Doctors;
using DoctorsClinic.Core.Dtos.Patients;
using DoctorsClinic.Core.Dtos.PrescriptionMedicines;
using Mapster;

namespace DoctorsClinic.Core.Mapster
{
    public class PrescriptionMap
    {
        public static void Configure()
        {

            TypeAdapterConfig<Prescription, PrescriptionDto>.NewConfig()
                .Map(d => d.DoctorName, s => s.Doctor != null ? s.Doctor.FullName : null)
                .Map(d => d.PatientName, s => s.Patient != null ? s.Patient.FullName : null);

            TypeAdapterConfig<PrescriptionDto, Prescription>.NewConfig()
                .Map(d => d.Notes, s => s.Notes ?? string.Empty)
                .Ignore(d => d.Appointment!)
                .Ignore(d => d.Doctor!)
                .Ignore(d => d.Patient!)
                .Ignore(d => d.PrescriptionMedicines!);

            TypeAdapterConfig<CreatePrescriptionDto, Prescription>.NewConfig()
                .Map(d => d.Notes, s => s.Notes ?? string.Empty)
                .Ignore(d => d.PrescriptionID)
                .Ignore(d => d.Appointment!)
                .Ignore(d => d.Doctor!)
                .Ignore(d => d.Patient!)
                .Ignore(d => d.PrescriptionMedicines!);

            TypeAdapterConfig<UpdatePrescriptionDto, Prescription>.NewConfig()
                .IgnoreIf((s, _) => s.AppointmentID == null, d => d.AppointmentID)
                .IgnoreIf((s, _) => s.DoctorID == null, d => d.DoctorID)
                .IgnoreIf((s, _) => s.PatientID == null, d => d.PatientID)
                .IgnoreIf((s, _) => s.Date == null, d => d.Date)
                .IgnoreIf((s, _) => string.IsNullOrWhiteSpace(s.Notes), d => d.Notes)
                .Map(d => d.Notes, s => s.Notes ?? string.Empty)
                .Ignore(d => d.Appointment!)
                .Ignore(d => d.Doctor!)
                .Ignore(d => d.Patient!)
                .Ignore(d => d.PrescriptionMedicines!);

            TypeAdapterConfig<Prescription, PrescriptionResponseDto>.NewConfig()
                .Map(d => d.Prescription, s => s.Adapt<PrescriptionDto>()!) 
                .Map(d => d.Appointment, _ => (AppointmentDto?)null)
                .Map(d => d.Doctor, _ => (DoctorDto?)null)
                .Map(d => d.Patient, _ => (PatientDto?)null)
                .Map(d => d.PrescriptionMedicines, _ => (List<PrescriptionMedicineDto>?)null);
        }
    }
}
