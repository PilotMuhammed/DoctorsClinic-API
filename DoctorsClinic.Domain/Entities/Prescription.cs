
namespace DoctorsClinic.Domain.Entities
{
    public class Prescription : BaseEntity<int>
    {
        public int AppointmentID { get; set; }
        public int DoctorID { get; set; }
        public int PatientID { get; set; }
        public DateTime? Date { get; set; } = DateTime.Now;
        public string? Notes { get; set; }

        public Appointment? Appointment { get; set; }
        public Doctor? Doctor { get; set; }
        public Patient? Patient { get; set; }
        public ICollection<PrescriptionMedicine>? PrescriptionMedicines { get; set; } 
    }
}
