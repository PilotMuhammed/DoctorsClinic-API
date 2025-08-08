using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoctorsClinic.Core.Dtos.Users
{
    public class UserDto
    {
        public int UserID { get; set; }
        public required string Username { get; set; }
        public required string Role { get; set; }        
        public int DoctorID { get; set; }
        public string? DoctorName { get; set; }  
    }
}
