using DoctorsClinic.Core.Dtos.Appointments;
using DoctorsClinic.Core.Dtos.Invoices;
using DoctorsClinic.Core.Dtos.MedicalRecords;
using DoctorsClinic.Core.Dtos.Prescriptions;

namespace DoctorsClinic.Core.Dtos.Patients
{
    public class PatientResponseDto
    {
        public required PatientDto Patient { get; set; }
        public List<AppointmentDto>? Appointments { get; set; }
        public List<MedicalRecordDto>? MedicalRecords { get; set; }
        public List<PrescriptionDto>? Prescriptions { get; set; }
        public List<InvoiceDto>? Invoices { get; set; }
    }
}