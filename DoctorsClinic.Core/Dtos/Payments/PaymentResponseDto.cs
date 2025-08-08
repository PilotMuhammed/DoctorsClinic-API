using DoctorsClinic.Core.Dtos.Invoices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoctorsClinic.Core.Dtos.Payments
{
    public class PaymentResponseDto
    {
        public required PaymentDto Payment { get; set; }
        public InvoiceDto? Invoice { get; set; }
    }
}
