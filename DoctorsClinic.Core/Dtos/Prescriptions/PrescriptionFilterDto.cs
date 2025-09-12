namespace DoctorsClinic.Core.Dtos.Prescriptions
{
    public class PrescriptionFilterDto
    {
        public int? AppointmentID { get; set; }
        public int? DoctorID { get; set; }
        public int? PatientID { get; set; }
        public string? Notes { get; set; }
    }
}