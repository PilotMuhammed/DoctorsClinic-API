using DoctorsClinic.Domain.Enums;

namespace DoctorsClinic.Core.Dtos.Payments
{
    public class UpdatePaymentDto
    {
        public int PaymentID { get; set; }
        public int? InvoiceID { get; set; }
        public decimal? Amount { get; set; }
        public PaymentMethod? PaymentMethod { get; set; }
    }
}