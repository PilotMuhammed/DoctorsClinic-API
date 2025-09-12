using DoctorsClinic.Domain.Enums;

namespace DoctorsClinic.Core.Dtos.Invoices
{
    public class CreateInvoiceDto
    {
        public int PatientID { get; set; }
        public int AppointmentID { get; set; }
        public decimal TotalAmount { get; set; }
        public required InvoiceStatus Status { get; set; }   
        public DateTime Date { get; set; }
    }
}