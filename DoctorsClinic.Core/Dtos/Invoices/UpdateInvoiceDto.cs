using DoctorsClinic.Domain.Enums;

namespace DoctorsClinic.Core.Dtos.Invoices
{
    public class UpdateInvoiceDto
    {
        public int InvoiceID { get; set; }
        public int? PatientID { get; set; }
        public int? AppointmentID { get; set; }
        public decimal? TotalAmount { get; set; }
        public InvoiceStatus? Status { get; set; }
        public DateTime? Date { get; set; }
    }
}