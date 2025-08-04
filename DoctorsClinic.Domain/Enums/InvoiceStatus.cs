using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoctorsClinic.Domain.Enums
{
    public enum InvoiceStatus
    {
        Unpaid = 1,
        Paid = 2,
        Pending = 3,
        Cancelled = 4
    }
}
