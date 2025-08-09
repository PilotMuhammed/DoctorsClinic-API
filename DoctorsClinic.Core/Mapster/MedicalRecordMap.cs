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

            TypeAdapterConfig<MedicalRecord, MedicalRecordDto>.NewConfig()
                .Map(d => d.PatientName, s => s.Patient != null ? s.Patient.FullName : null)
                .Map(d => d.DoctorName, s => s.Doctor != null ? s.Doctor.FullName : null);

            TypeAdapterConfig<MedicalRecordDto, MedicalRecord>.NewConfig()
                .Ignore(d => d.Patient)
                .Ignore(d => d.Doctor);

            TypeAdapterConfig<CreateMedicalRecordDto, MedicalRecord>.NewConfig()
                .Ignore(d => d.RecordID)
                .Ignore(d => d.Patient)
                .Ignore(d => d.Doctor);

            TypeAdapterConfig<UpdateMedicalRecordDto, MedicalRecord>.NewConfig()
                .IgnoreIf((s, _) => s.PatientID == null, d => d.PatientID)
                .IgnoreIf((s, _) => s.DoctorID == null, d => d.DoctorID)
                .IgnoreIf((s, _) => string.IsNullOrWhiteSpace(s.Diagnosis), d => d.Diagnosis)
                .IgnoreIf((s, _) => s.Date == null, d => d.Date)
                .IgnoreIf((s, _) => string.IsNullOrWhiteSpace(s.Notes), d => d.Notes)
                .Ignore(d => d.Patient)
                .Ignore(d => d.Doctor);

            TypeAdapterConfig<MedicalRecord, MedicalRecordResponseDto>.NewConfig()
                .Map(d => d.MedicalRecord, s => s.Adapt<MedicalRecordDto>()!) 
                .Map(d => d.Patient, _ => (PatientDto?)null)
                .Map(d => d.Doctor, _ => (DoctorDto?)null);
        }
    }
}
