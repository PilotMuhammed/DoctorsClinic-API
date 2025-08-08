using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoctorsClinic.Core.Dtos.Invoices
{
    public class InvoiceDto
    {
        public int InvoiceID { get; set; }
        public int PatientID { get; set; }
        public string? PatientName { get; set; }        
        public int AppointmentID { get; set; }
        public decimal TotalAmount { get; set; }
        public required string Status { get; set; }             
        public DateTime Date { get; set; }
    }
}
