using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DoctorsClinic.Domain.Enums;

namespace DoctorsClinic.Domain.Entities
{
    public class Payment
    {
        public int PaymentID { get; set; }
        public int InvoiceID { get; set; }
        public decimal Amount { get; set; }
        public DateTime Date { get; set; }
        public PaymentMethod PaymentMethod { get; set; }

        
        public Invoice? Invoice { get; set; }
    }
}
