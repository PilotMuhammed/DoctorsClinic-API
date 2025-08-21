using DoctorsClinic.Core.Dtos.Permission;
using DoctorsClinic.Core.Dtos.Role;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoctorsClinic.Core.Dtos.Account
{
    public class LoginUserResponse
    {
        public int UserID { get; set; }
        public string? Username { get; set; } 
        public GetRole? Role { get; set; } 
        public int? DoctorID { get; set; }                
        public required string Token { get; set; } 
        public List<GetEnum>? Permissions { get; set; }
    }
}