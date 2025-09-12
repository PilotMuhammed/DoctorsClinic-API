namespace DoctorsClinic.Core.Dtos.MedicalRecords
{
    public class UpdateMedicalRecordDto
    {
        public int RecordID { get; set; }
        public int? PatientID { get; set; }
        public int? DoctorID { get; set; }
        public string? Diagnosis { get; set; }
        public string? Notes { get; set; }
    }
}