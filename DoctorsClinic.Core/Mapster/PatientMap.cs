using DoctorsClinic.Core.Dtos.Appointments;
using DoctorsClinic.Core.Dtos.Invoices;
using DoctorsClinic.Core.Dtos.MedicalRecords;
using DoctorsClinic.Core.Dtos.Patients;
using DoctorsClinic.Core.Dtos.Prescriptions;
using DoctorsClinic.Domain.Entities;
using DoctorsClinic.Domain.Enums; 
using Mapster;

namespace DoctorsClinic.Core.Mapster
{
    public class PatientMap
    {
        public static void Configure()
        {

            TypeAdapterConfig<Patient, PatientDto>.NewConfig()
                .Map(d => d.Gender, s => s.Gender.ToString());

            TypeAdapterConfig<PatientDto, Patient>.NewConfig()
                .Map(d => d.Gender, s => ParseGender(s.Gender))
                .Ignore(d => d.Appointments)
                .Ignore(d => d.MedicalRecords)
                .Ignore(d => d.Prescriptions)
                .Ignore(d => d.Invoices);

            TypeAdapterConfig<CreatePatientDto, Patient>.NewConfig()
                .Map(d => d.Gender, s => ParseGender(s.Gender))
                .Ignore(d => d.PatientID)
                .Ignore(d => d.Appointments)
                .Ignore(d => d.MedicalRecords)
                .Ignore(d => d.Prescriptions)
                .Ignore(d => d.Invoices);

            TypeAdapterConfig<UpdatePatientDto, Patient>.NewConfig()
                .IgnoreIf((s, _) => string.IsNullOrWhiteSpace(s.FullName), d => d.FullName)
                .IgnoreIf((s, _) => string.IsNullOrWhiteSpace(s.Gender), d => d.Gender)
                .IgnoreIf((s, _) => s.DOB == null, d => d.DOB)
                .IgnoreIf((s, _) => string.IsNullOrWhiteSpace(s.Phone), d => d.Phone)
                .IgnoreIf((s, _) => string.IsNullOrWhiteSpace(s.Email), d => d.Email)
                .IgnoreIf((s, _) => string.IsNullOrWhiteSpace(s.Address), d => d.Address)
                .Map(d => d.Gender, s => string.IsNullOrEmpty(s.Gender) ? default : ParseGender(s.Gender))
                .Ignore(d => d.Appointments)
                .Ignore(d => d.MedicalRecords)
                .Ignore(d => d.Prescriptions)
                .Ignore(d => d.Invoices);

            TypeAdapterConfig<Patient, PatientResponseDto>.NewConfig()
                .Map(d => d.Patient, s => s.Adapt<PatientDto>()!)
                .Map(d => d.Appointments, _ => (List<AppointmentDto>?)null)
                .Map(d => d.MedicalRecords, _ => (List<MedicalRecordDto>?)null)
                .Map(d => d.Prescriptions, _ => (List<PrescriptionDto>?)null)
                .Map(d => d.Invoices, _ => (List<InvoiceDto>?)null);
        }

        private static Gender ParseGender(string? gender)
        {
            return Enum.TryParse<Gender>(gender, true, out var value) ? value : default;
        }
    }
}
