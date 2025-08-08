using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoctorsClinic.Core.Dtos.Helper
{
    public interface IPaginationQuery
    {
        int? Page { get; set; }
        int? PageSize { get; set; }
    }
}
