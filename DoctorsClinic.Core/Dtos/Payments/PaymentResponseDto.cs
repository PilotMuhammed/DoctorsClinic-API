using DoctorsClinic.Core.Dtos.Invoices;

namespace DoctorsClinic.Core.Dtos.Payments
{
    public class PaymentResponseDto
    {
        public PaymentDto? Payment { get; set; }
        public InvoiceDto? Invoice { get; set; }
    }
}