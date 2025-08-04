using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DoctorsClinic.Domain.Enums;

namespace DoctorsClinic.Domain.Entities
{
    public class User
    {
        public int UserID { get; set; }
        public string? Username { get; set; }
        public string? PasswordHash { get; set; }
        public UserRole Role { get; set; }
        public int DoctorID { get; set; }


        public Doctor? Doctor { get; set; }
    }
}
