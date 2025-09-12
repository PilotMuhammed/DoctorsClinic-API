using DoctorsClinic.Domain.Enums;

namespace DoctorsClinic.Core.Dtos.Payments
{
    public class PaymentFilterDto
    {
        public int? InvoiceID { get; set; }
        public decimal? Amount { get; set; }
        public PaymentMethod? PaymentMethod { get; set; }
    }
}