using DoctorsClinic.Core.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoctorsClinic.Core.Dtos.Users
{
    public class CreateUserDto
    {
        public required string Username { get; set; }
        [PasswordValidation]
        public required string Password { get; set; }      
        public required string Role { get; set; }          
        public int DoctorID { get; set; }
    }
}
