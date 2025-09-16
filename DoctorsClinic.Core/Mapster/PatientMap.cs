using DoctorsClinic.Core.Dtos.Appointments;
using DoctorsClinic.Core.Dtos.Invoices;
using DoctorsClinic.Core.Dtos.MedicalRecords;
using DoctorsClinic.Core.Dtos.Patients;
using DoctorsClinic.Core.Dtos.Prescriptions;
using DoctorsClinic.Domain.Entities;
using Mapster;

namespace DoctorsClinic.Core.Mapster
{
    public class PatientMap
    {
        public static void Configure()
        {
            TypeAdapterConfig<CreatePatientDto, Patient>.NewConfig()
                .Ignore(dest => dest.Appointments!)
                .Ignore(dest => dest.MedicalRecords!)
                .Ignore(dest => dest.Prescriptions!)
                .Ignore(dest => dest.Invoices!);

            TypeAdapterConfig<Patient, PatientResponseDto>.NewConfig()
                .Map(dest => dest.Patient, src => src.Adapt<PatientDto>()!)
                .Map(dest => dest.Appointments, src => src.Appointments.Adapt<List<AppointmentDto>>())
                .Map(dest => dest.MedicalRecords, src => src.MedicalRecords.Adapt<List<MedicalRecordDto>>())
                .Map(dest => dest.Prescriptions, src => src.Prescriptions.Adapt<List<PrescriptionDto>>())
                .Map(dest => dest.Invoices, src => src.Invoices.Adapt<List<InvoiceDto>>());

            TypeAdapterConfig<Patient, PatientDto>.NewConfig();

            TypeAdapterConfig<UpdatePatientDto, Patient>.NewConfig()
                .IgnoreIf((src, dest) => string.IsNullOrWhiteSpace(src.FullName), dest => dest.FullName)
                .IgnoreIf((src, dest) => src.Gender == null, dest => dest.Gender)
                .IgnoreIf((src, dest) => src.DateOfBirth == null, dest => dest.DateOfBirth)
                .IgnoreIf((src, dest) => string.IsNullOrWhiteSpace(src.Phone), dest => dest.Phone!)
                .IgnoreIf((src, dest) => string.IsNullOrWhiteSpace(src.Address), dest => dest.Address!)
                .Ignore(dest => dest.Appointments!)
                .Ignore(dest => dest.MedicalRecords!)
                .Ignore(dest => dest.Prescriptions!)
                .Ignore(dest => dest.Invoices!);
        }
    }
}
