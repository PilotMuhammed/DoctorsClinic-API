using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoctorsClinic.Domain.Entities
{
    public class Medicine
    {
        public int MedicineID { get; set; }
        public required string Name { get; set; }
        public required string Description { get; set; }
        public required string Type { get; set; }

        public ICollection<PrescriptionMedicine>? PrescriptionMedicines { get; set; } 
    }
}
