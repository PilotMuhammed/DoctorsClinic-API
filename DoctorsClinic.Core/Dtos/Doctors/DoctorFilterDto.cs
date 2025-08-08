using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoctorsClinic.Core.Dtos.Doctors
{
    public class DoctorFilterDto
    {
        public string? FullName { get; set; }
        public int? SpecialtyID { get; set; }
        public string? Phone { get; set; }
        public string? Email { get; set; }
    }
}
