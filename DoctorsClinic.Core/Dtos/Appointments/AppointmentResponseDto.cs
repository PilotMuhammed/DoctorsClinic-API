using DoctorsClinic.Core.Dtos.Doctors;
using DoctorsClinic.Core.Dtos.Invoices;
using DoctorsClinic.Core.Dtos.Patients;
using DoctorsClinic.Core.Dtos.Prescriptions;

namespace DoctorsClinic.Core.Dtos.Appointments
{
    public class AppointmentResponseDto
    {
        public AppointmentDto? Appointment { get; set; }
        public PatientDto? Patient { get; set; }
        public DoctorDto? Doctor { get; set; }
        public List<PrescriptionDto>? Prescriptions { get; set; }
        public InvoiceDto? Invoice { get; set; }
    }
}