using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoctorsClinic.Core.Dtos.Patients
{
    public class PatientFilterDto
    {
        public string? FullName { get; set; }
        public string? Gender { get; set; }
        public DateTime? DOBFrom { get; set; }
        public DateTime? DOBTo { get; set; }
        public string? Phone { get; set; }
        public string? Email { get; set; }
    }
}
