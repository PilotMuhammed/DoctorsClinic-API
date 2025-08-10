using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoctorsClinic.Core.Dtos.Account
{
    public class LoginUser
    {
        public required string Username { get; set; }
        public required string Password { get; set; } 
    }
}