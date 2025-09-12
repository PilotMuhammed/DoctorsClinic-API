using DoctorsClinic.Domain.Enums;

namespace DoctorsClinic.Domain.Entities
{
    public class Invoice : BaseEntity<int>
    {
        public int PatientID { get; set; }
        public int AppointmentID { get; set; }
        public decimal TotalAmount { get; set; }
        public InvoiceStatus Status { get; set; }
        public DateTime Date { get; set; }

        public Patient? Patient { get; set; }
        public Appointment? Appointment { get; set; }
        public ICollection<Payment>? Payments { get; set; } 
    }
}
