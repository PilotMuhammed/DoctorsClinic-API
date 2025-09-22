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
            TypeAdapterConfig<CreateDoctorDto, Doctor>.NewConfig()
                .Ignore(dest => dest.Appointments!)
                .Ignore(dest => dest.MedicalRecords!)
                .Ignore(dest => dest.Prescriptions!);

            TypeAdapterConfig<Doctor, DoctorResponseDto>.NewConfig()
                .Map(dest => dest.Doctor, src => src.Adapt<DoctorDto>())
                .Map(dest => dest.Appointments, src => src.Appointments.Adapt<List<AppointmentDto>>())
                .Map(dest => dest.MedicalRecords, src => src.MedicalRecords.Adapt<List<MedicalRecordDto>>())
                .Map(dest => dest.Prescriptions, src => src.Prescriptions.Adapt<List<PrescriptionDto>>());

            TypeAdapterConfig<Doctor, DoctorDto>.NewConfig()
                .Map(dest => dest.SpecialtyName, src => src.Specialty!.Name)
                .Map(dest => dest.UserName, src => src.User!.UserName);

            TypeAdapterConfig<UpdateDoctorDto, Doctor>.NewConfig()
                .IgnoreIf((src, dest) => string.IsNullOrWhiteSpace(src.FullName), dest => dest.FullName)
                .IgnoreIf((src, dest) => src.SpecialtyID == null, dest => dest.SpecialtyID)
                .IgnoreIf((src, dest) => string.IsNullOrWhiteSpace(src.Phone), dest => dest.Phone!)
                .IgnoreIf((src, dest) => string.IsNullOrWhiteSpace(src.Email), dest => dest.Email!)
                .IgnoreIf((src, dest) => src.UserID == null, dest => dest.UserID!)
                .Ignore(dest => dest.Appointments!)
                .Ignore(dest => dest.MedicalRecords!)
                .Ignore(dest => dest.Prescriptions!);
        }
    }
}
