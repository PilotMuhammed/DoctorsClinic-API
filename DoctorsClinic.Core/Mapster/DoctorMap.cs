using DoctorsClinic.Core.Dtos.Appointments;
using DoctorsClinic.Core.Dtos.Doctors;
using DoctorsClinic.Core.Dtos.MedicalRecords;
using DoctorsClinic.Core.Dtos.Prescriptions;
using DoctorsClinic.Domain.Entities;
using Mapster;

namespace DoctorsClinic.Core.Mapster
{
    public class DoctorMap
    {
        public static void Configure()
        {

            TypeAdapterConfig<Doctor, DoctorDto>.NewConfig()
                .Map(d => d.SpecialtyName, s => s.Specialty != null ? s.Specialty.Name : null)
                .Map(d => d.UserName, s => s.User != null ? s.User.Username : null);

            TypeAdapterConfig<DoctorDto, Doctor>.NewConfig()
                .Ignore(d => d.Specialty!)
                .Ignore(d => d.User!)
                .Ignore(d => d.Appointments!)
                .Ignore(d => d.MedicalRecords!)
                .Ignore(d => d.Prescriptions!);

            TypeAdapterConfig<CreateDoctorDto, Doctor>.NewConfig()
                .Ignore(d => d.DoctorID)
                .Ignore(d => d.Specialty!)
                .Ignore(d => d.User!)
                .Ignore(d => d.Appointments!)
                .Ignore(d => d.MedicalRecords!)
                .Ignore(d => d.Prescriptions!);

            TypeAdapterConfig<UpdateDoctorDto, Doctor>.NewConfig()
                .IgnoreIf((s, _) => string.IsNullOrWhiteSpace(s.FullName), d => d.FullName)
                .IgnoreIf((s, _) => s.SpecialtyID == null, d => d.SpecialtyID)
                .IgnoreIf((s, _) => string.IsNullOrWhiteSpace(s.Phone), d => d.Phone!)
                .IgnoreIf((s, _) => string.IsNullOrWhiteSpace(s.Email), d => d.Email!)
                .IgnoreIf((s, _) => s.UserID == null, d => d.UserID!)
                .Ignore(d => d.Specialty!)
                .Ignore(d => d.User!)
                .Ignore(d => d.Appointments!)
                .Ignore(d => d.MedicalRecords!)
                .Ignore(d => d.Prescriptions!);

            TypeAdapterConfig<Doctor, DoctorResponseDto>.NewConfig()
                .Map(d => d.Doctor, s => s.Adapt<DoctorDto>()!) 
                .Map(d => d.Appointments, _ => (List<AppointmentDto>?)null)
                .Map(d => d.MedicalRecords, _ => (List<MedicalRecordDto>?)null)
                .Map(d => d.Prescriptions, _ => (List<PrescriptionDto>?)null);
        }
    }
}
