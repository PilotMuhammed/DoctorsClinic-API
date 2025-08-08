using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoctorsClinic.Core.Dtos.Payments
{
    public class UpdatePaymentDto
    {
        public int PaymentID { get; set; }
        public int? InvoiceID { get; set; }
        public decimal? Amount { get; set; }
        public DateTime? Date { get; set; }
        public string? PaymentMethod { get; set; }
    }
}
