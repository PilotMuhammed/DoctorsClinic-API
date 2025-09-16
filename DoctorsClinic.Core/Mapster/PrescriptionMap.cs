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
            TypeAdapterConfig<CreatePrescriptionDto, Prescription>.NewConfig()
                .Ignore(dest => dest.PrescriptionMedicines!);

            TypeAdapterConfig<Prescription, PrescriptionResponseDto>.NewConfig()
                .Map(dest => dest.Prescription, src => src.Adapt<PrescriptionDto>())
                .Map(dest => dest.Appointment, src => src.Appointment.Adapt<AppointmentDto>())
                .Map(dest => dest.Doctor, src => src.Doctor.Adapt<DoctorDto>())
                .Map(dest => dest.Patient, src => src.Patient.Adapt<PatientDto>())
                .Map(dest => dest.PrescriptionMedicines, src => src.PrescriptionMedicines.Adapt<List<PrescriptionMedicineDto>>());

            TypeAdapterConfig<Prescription, PrescriptionDto>.NewConfig();

            TypeAdapterConfig<UpdatePrescriptionDto, Prescription>.NewConfig()
                .IgnoreIf((src, dest) => src.AppointmentID == null, dest => dest.AppointmentID)
                .IgnoreIf((src, dest) => src.DoctorID == null, dest => dest.DoctorID)
                .IgnoreIf((src, dest) => src.PatientID == null, dest => dest.PatientID)
                .IgnoreIf((src, dest) => string.IsNullOrWhiteSpace(src.Notes), dest => dest.Notes!)
                .Ignore(dest => dest.PrescriptionMedicines!);
        }
    }
}
