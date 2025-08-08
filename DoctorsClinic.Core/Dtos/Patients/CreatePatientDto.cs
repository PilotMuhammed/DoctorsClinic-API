using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoctorsClinic.Core.Dtos.Patients
{
    public class CreatePatientDto
    {
        public required string FullName { get; set; }
        public required string Gender { get; set; } 
        public DateTime DOB { get; set; }
        public string? Phone { get; set; }
        public string? Email { get; set; }
        public string? Address { get; set; }
    }
}
