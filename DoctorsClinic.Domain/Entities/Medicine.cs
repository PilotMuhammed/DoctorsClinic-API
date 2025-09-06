
namespace DoctorsClinic.Domain.Entities
{
    public class Medicine : BaseEntity<int>
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string Type { get; set; }

        public ICollection<PrescriptionMedicine>? PrescriptionMedicines { get; set; } 
    }
}
