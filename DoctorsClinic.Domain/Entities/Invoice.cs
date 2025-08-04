using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DoctorsClinic.Domain.Enums;

namespace DoctorsClinic.Domain.Entities
{
    public class Invoice
    {
        public int InvoiceID { get; set; }
        public int PatientID { get; set; }
        public int AppointmentID { get; set; }
        public decimal TotalAmount { get; set; }
        public InvoiceStatus Status { get; set; }
        public DateTime Date { get; set; }

        
        public Patient? Patient { get; set; }
        public Appointment? Appointment { get; set; }
        public ICollection<Payment> Payments { get; set; } = new List<Payment>();
    }
}
