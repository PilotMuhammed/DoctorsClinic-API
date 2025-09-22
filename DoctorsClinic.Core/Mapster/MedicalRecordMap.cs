using DoctorsClinic.Core.Dtos.Doctors;
using DoctorsClinic.Core.Dtos.MedicalRecords;
using DoctorsClinic.Core.Dtos.Patients;
using DoctorsClinic.Domain.Entities;
using Mapster;

namespace DoctorsClinic.Core.Mapster
{
    public class MedicalRecordMap
    {
        public static void Configure()
        {
            TypeAdapterConfig<CreateMedicalRecordDto, MedicalRecord>.NewConfig();

            TypeAdapterConfig<MedicalRecord, MedicalRecordResponseDto>.NewConfig()
                .Map(dest => dest.MedicalRecord, src => src.Adapt<MedicalRecordDto>())
                .Map(dest => dest.Patient, src => src.Patient.Adapt<PatientDto>())
                .Map(dest => dest.Doctor, src => src.Doctor.Adapt<DoctorDto>());

            TypeAdapterConfig<MedicalRecord, MedicalRecordDto>.NewConfig()
                .Map(dest => dest.PatientName, src => src.Patient!.FullName)
                .Map(dest => dest.DoctorName, src => src.Doctor!.FullName);

            TypeAdapterConfig<UpdateMedicalRecordDto, MedicalRecord>.NewConfig()
                .IgnoreIf((src, dest) => src.PatientID == null, dest => dest.PatientID)
                .IgnoreIf((src, dest) => src.DoctorID == null, dest => dest.DoctorID)
                .IgnoreIf((src, dest) => string.IsNullOrWhiteSpace(src.Diagnosis), dest => dest.Diagnosis!)
                .IgnoreIf((src, dest) => string.IsNullOrWhiteSpace(src.Notes), dest => dest.Notes!);
        }
    }
}
