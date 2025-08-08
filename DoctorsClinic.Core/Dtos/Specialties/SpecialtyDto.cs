using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoctorsClinic.Core.Dtos.Specialties
{
    public class SpecialtyDto
    {
        public int SpecialtyID { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
    }
}
