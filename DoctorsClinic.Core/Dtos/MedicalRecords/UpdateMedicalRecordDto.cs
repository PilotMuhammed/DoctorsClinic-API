namespace DoctorsClinic.Core.Dtos.MedicalRecords
{
    public class UpdateMedicalRecordDto
    {
        public int Id { get; set; }
        public int? PatientID { get; set; }
        public int? DoctorID { get; set; }
        public string? Diagnosis { get; set; }
        public string? Notes { get; set; }
    }
}