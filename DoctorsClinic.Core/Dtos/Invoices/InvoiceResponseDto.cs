using DoctorsClinic.Core.Dtos.Appointments;
using DoctorsClinic.Core.Dtos.Patients;
using DoctorsClinic.Core.Dtos.Payments;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoctorsClinic.Core.Dtos.Invoices
{
    public class InvoiceResponseDto
    {
        public required InvoiceDto Invoice { get; set; }
        public PatientDto? Patient { get; set; }
        public AppointmentDto? Appointment { get; set; }
        public List<PaymentDto>? Payments { get; set; }
    }
}
