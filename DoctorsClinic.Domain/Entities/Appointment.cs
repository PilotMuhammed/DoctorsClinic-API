using DoctorsClinic.Domain.Enums;

namespace DoctorsClinic.Domain.Entities
{
    public class Appointment : BaseEntity<int>
    {
        public int PatientID { get; set; }
        public int DoctorID { get; set; }
        public DateTime? AppointmentDate { get; set; } = DateTime.Now;
        public AppointmentStatus Status { get; set; }
        public string? Notes { get; set; }
        
        public Patient? Patient { get; set; }
        public Doctor? Doctor { get; set; }
        public ICollection<Prescription>? Prescriptions { get; set; } 
        public Invoice? Invoice { get; set; }
    }
}
