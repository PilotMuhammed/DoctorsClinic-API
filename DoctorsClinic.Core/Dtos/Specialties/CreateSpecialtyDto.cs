using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoctorsClinic.Core.Dtos.Specialties
{
    public class CreateSpecialtyDto
    {
        public required string Name { get; set; }
        public string? Description { get; set; }
    }
}
