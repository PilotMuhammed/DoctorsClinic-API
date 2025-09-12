using DoctorsClinic.Domain.Enums;

namespace DoctorsClinic.Core.Dtos.Invoices
{
    public class InvoiceFilterDto
    {
        public int? PatientID { get; set; }
        public int? AppointmentID { get; set; }
        public InvoiceStatus? Status { get; set; }
        public DateTime? Date { get; set; }
        public DateTime? CreatedAt { get; set; }
    }
}