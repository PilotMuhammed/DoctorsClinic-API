namespace DoctorsClinic.Core.Dtos.MedicalRecords
{
    public class MedicalRecordFilterDto
    {
        public int? PatientID { get; set; }
        public int? DoctorID { get; set; }
        public string? Diagnosis { get; set; }
        public string? Notes { get; set; }
    }
}