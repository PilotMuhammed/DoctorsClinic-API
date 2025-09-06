
namespace DoctorsClinic.Domain.Entities
{
    public class PrescriptionMedicine : BaseEntity<int>
    {
        public int PrescriptionID { get; set; }
        public int MedicineID { get; set; }
        public string Dose { get; set; }
        public string Duration { get; set; }
        public string Instructions { get; set; }

        public Prescription? Prescription { get; set; }
        public Medicine? Medicine { get; set; }
    }
}
