namespace DoctorsClinic.Core.Dtos.PrescriptionMedicines
{
    public class PrescriptionMedicineFilterDto
    {
        public int? PrescriptionID { get; set; }
        public int? MedicineID { get; set; }
        public string? Dose { get; set; }
        public string? Duration { get; set; }
        public string? Instructions { get; set; }
    }
}