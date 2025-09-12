namespace DoctorsClinic.Core.Dtos.MedicalRecords
{
    public class CreateMedicalRecordDto
    {
        public int PatientID { get; set; }
        public int DoctorID { get; set; }
        public required string Diagnosis { get; set; }
        public string? Notes { get; set; }
    }
}