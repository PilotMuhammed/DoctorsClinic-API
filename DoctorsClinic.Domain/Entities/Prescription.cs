using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoctorsClinic.Domain.Entities
{
    public class Prescription
    {
        public int PrescriptionID { get; set; }
        public int AppointmentID { get; set; }
        public int DoctorID { get; set; }
        public int PatientID { get; set; }
        public DateTime Date { get; set; }
        public required string Notes { get; set; }

        
        public required Appointment Appointment { get; set; }
        public required Doctor Doctor { get; set; }
        public required Patient Patient { get; set; }
        public required ICollection<PrescriptionMedicine> PrescriptionMedicines { get; set; } 
    }
}
