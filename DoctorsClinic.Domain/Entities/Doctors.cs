using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoctorsClinic.Domain.Entities
{
    public class Doctor
    {
        public int DoctorID { get; set; }
        public required string FullName { get; set; }
        public int SpecialtyID { get; set; }
        public required string Phone { get; set; }
        public required string Email { get; set; }
        public int UserID { get; set; }

        
        public required Specialty Specialty { get; set; }
        public required User User { get; set; }
        public required ICollection<Appointment> Appointments { get; set; }  
        public required ICollection<MedicalRecord> MedicalRecords { get; set; }  
        public required ICollection<Prescription> Prescriptions { get; set; } 
    }
}
