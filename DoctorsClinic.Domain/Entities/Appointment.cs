using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DoctorsClinic.Domain.Enums;

namespace DoctorsClinic.Domain.Entities
{
    public class Appointment
    {
        public int AppointmentID { get; set; }
        public int PatientID { get; set; }
        public int DoctorID { get; set; }
        public DateTime AppointmentDate { get; set; }
        public AppointmentStatus Status { get; set; }
        public string? Notes { get; set; }
        
        
        public Patient? Patient { get; set; }
        public Doctor? Doctor { get; set; }
        public ICollection<Prescription> Prescriptions { get; set; } = new List<Prescription>();
        public Invoice? Invoice { get; set; }
    }
}
