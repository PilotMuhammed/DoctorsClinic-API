using DoctorsClinic.Domain.Enums;

namespace DoctorsClinic.Core.Dtos.Payments
{
    public class CreatePaymentDto
    {
        public int InvoiceID { get; set; }
        public decimal Amount { get; set; }
        public required PaymentMethod PaymentMethod { get; set; }   
    }
}