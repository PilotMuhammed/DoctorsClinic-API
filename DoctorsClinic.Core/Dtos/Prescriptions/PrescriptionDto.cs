namespace DoctorsClinic.Core.Dtos.Prescriptions
{
    public class PrescriptionDto
    {
        public int Id { get; set; }
        public int AppointmentID { get; set; }
        public int DoctorID { get; set; }
        public string? DoctorName { get; set; }
        public int PatientID { get; set; }
        public string? PatientName { get; set; }
        public string? Notes { get; set; }
        public DateTime? CreatedAt { get; set; }
    }
}