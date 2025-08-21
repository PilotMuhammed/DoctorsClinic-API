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
        public string? Phone { get; set; }
        public string? Email { get; set; }
        public int? UserID { get; set; }

        public Specialty? Specialty { get; set; }
        public User? User { get; set; }
        public ICollection<Appointment>? Appointments { get; set; }  
        public ICollection<MedicalRecord>? MedicalRecords { get; set; }  
        public ICollection<Prescription>? Prescriptions { get; set; } 
    }
}
