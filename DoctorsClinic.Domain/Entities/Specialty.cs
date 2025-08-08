using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoctorsClinic.Domain.Entities
{
    public class Specialty
    {
        public int SpecialtyID { get; set; }
        public required string Name { get; set; }
        public required string Description { get; set; }

        
        public required ICollection<Doctor> Doctors { get; set; } 
    }
}
