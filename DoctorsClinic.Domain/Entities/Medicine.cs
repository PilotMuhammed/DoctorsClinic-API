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
        public string? Name { get; set; }
        public string? Description { get; set; }
        public string? Type { get; set; }

        
        public ICollection<PrescriptionMedicine> PrescriptionMedicines { get; set; } = new List<PrescriptionMedicine>();
    }
}
