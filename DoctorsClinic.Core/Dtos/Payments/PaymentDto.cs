using DoctorsClinic.Domain.Enums;

namespace DoctorsClinic.Core.Dtos.Payments
{
    public class PaymentDto
    {
        public int Id { get; set; }
        public int? InvoiceID { get; set; }
        public decimal? Amount { get; set; }
        public PaymentMethod? PaymentMethod { get; set; }
        public DateTime? CreatedAt { get; set; }
    }
}