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

        public Appointment? Appointment { get; set; }
        public Doctor? Doctor { get; set; }
        public Patient? Patient { get; set; }
        public ICollection<PrescriptionMedicine>? PrescriptionMedicines { get; set; } 
    }
}
