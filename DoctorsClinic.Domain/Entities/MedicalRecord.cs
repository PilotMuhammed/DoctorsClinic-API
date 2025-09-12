
namespace DoctorsClinic.Domain.Entities
{
    public class MedicalRecord : BaseEntity<int>
    {
        public int PatientID { get; set; }
        public int DoctorID { get; set; }
        public string? Diagnosis { get; set; }
        public string? Notes { get; set; }

        public Patient? Patient { get; set; }
        public Doctor? Doctor { get; set; }
    }
}
