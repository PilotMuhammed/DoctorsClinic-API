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
        public string? Name { get; set; }
        public string? Description { get; set; }

        
        public ICollection<Doctor> Doctors { get; set; } = new List<Doctor>();
    }
}
