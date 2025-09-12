namespace DoctorsClinic.Core.Dtos.MedicalRecords
{
    public class MedicalRecordDto
    {
        public int RecordID { get; set; }
        public int? PatientID { get; set; }
        public string? PatientName { get; set; }    
        public int? DoctorID { get; set; }
        public string? DoctorName { get; set; }     
        public string? Diagnosis { get; set; }
        public string? Notes { get; set; }
        public DateTime? CreatedAt { get; set; }
    }
}