using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoctorsClinic.Core.Dtos.Users
{
    public class UserFilterDto
    {
        public string? Username { get; set; }
        public string? Role { get; set; }
        public int? DoctorID { get; set; }
    }
}
