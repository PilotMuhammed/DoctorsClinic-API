using DoctorsClinic.Domain.Enums;

namespace DoctorsClinic.Domain.Entities
{
    public class Payment : BaseEntity<int>
    {
        public int InvoiceID { get; set; }
        public decimal Amount { get; set; }
        public PaymentMethod PaymentMethod { get; set; }

        public Invoice? Invoice { get; set; }
    }
}
