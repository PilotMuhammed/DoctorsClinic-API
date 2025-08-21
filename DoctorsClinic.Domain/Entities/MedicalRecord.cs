using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoctorsClinic.Domain.Entities
{
    public class MedicalRecord
    {
        public int RecordID { get; set; }
        public int PatientID { get; set; }
        public int DoctorID { get; set; }
        public required string Diagnosis { get; set; }
        public DateTime Date { get; set; }
        public required string Notes { get; set; }

        public Patient? Patient { get; set; }
        public Doctor? Doctor { get; set; }
    }
}
