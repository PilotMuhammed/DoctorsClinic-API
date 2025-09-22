using DoctorsClinic.Domain.Enums;

namespace DoctorsClinic.Core.Dtos.Invoices
{
    public class InvoiceDto
    {
        public int Id { get; set; }
        public int? PatientID { get; set; }
        public string? PatientName { get; set; }        
        public int? AppointmentID { get; set; }
        public decimal? TotalAmount { get; set; }
        public InvoiceStatus? Status { get; set; }             
        public DateTime? Date { get; set; }
        public DateTime? CreatedAt { get; set; }
    }
}