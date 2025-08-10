using DoctorsClinic.Core.Dtos.Permission;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoctorsClinic.Core.Dtos.Role
{
    public class GetRole
    {
        public int Id { get; set; }   
        public string? Name { get; set; } 
    }
}