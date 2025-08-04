using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoctorsClinic.Domain.Entities
{
    public class PrescriptionMedicine
    {
        public int ID { get; set; }
        public int PrescriptionID { get; set; }
        public int MedicineID { get; set; }
        public string? Dose { get; set; }
        public string? Duration { get; set; }
        public string? Instructions { get; set; }

        
        public Prescription? Prescription { get; set; }
        public Medicine? Medicine { get; set; }
    }
}
