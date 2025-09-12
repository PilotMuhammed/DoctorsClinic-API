using DoctorsClinic.Core.Dtos.Appointments;
using DoctorsClinic.Core.Dtos.Patients;
using DoctorsClinic.Core.Dtos.Payments;

namespace DoctorsClinic.Core.Dtos.Invoices
{
    public class InvoiceResponseDto
    {
        public InvoiceDto? Invoice { get; set; }
        public PatientDto? Patient { get; set; }
        public AppointmentDto? Appointment { get; set; }
        public List<PaymentDto>? Payments { get; set; }
    }
}