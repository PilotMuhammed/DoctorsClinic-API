using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoctorsClinic.Core.Dtos.Invoices
{
    public class CreateInvoiceDto
    {
        public int PatientID { get; set; }
        public int AppointmentID { get; set; }
        public decimal TotalAmount { get; set; }
        public required string Status { get; set; }   
        public DateTime Date { get; set; }
    }
}
