using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoctorsClinic.Core.Dtos.Invoices
{
    public class UpdateInvoiceDto
    {
        public int InvoiceID { get; set; }
        public int? PatientID { get; set; }
        public int? AppointmentID { get; set; }
        public decimal? TotalAmount { get; set; }
        public string? Status { get; set; }
        public DateTime? Date { get; set; }
    }
}
