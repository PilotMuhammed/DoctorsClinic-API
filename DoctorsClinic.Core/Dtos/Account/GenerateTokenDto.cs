using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoctorsClinic.Core.Dtos.Account
{
    public class GenerateTokenDto
    {
        public int UserId { get; set; }  
        public required string UserName { get; set; } 
        public required string RoleName { get; set; } 
        public IReadOnlyCollection<string>? RolePermissions { get; set; } 
        public IReadOnlyCollection<string>? UserPermissions { get; set; } 
    }
}