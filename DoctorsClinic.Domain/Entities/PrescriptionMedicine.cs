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
        public required string Dose { get; set; }
        public required string Duration { get; set; }
        public required string Instructions { get; set; }

        public Prescription? Prescription { get; set; }
        public Medicine? Medicine { get; set; }
    }
}
