namespace DoctorsClinic.Core.Dtos.PrescriptionMedicines
{
    public class CreatePrescriptionMedicineDto
    {
        public int PrescriptionID { get; set; }
        public int MedicineID { get; set; }
        public required string Dose { get; set; }
        public required string Duration { get; set; }
        public string? Instructions { get; set; }
    }
}