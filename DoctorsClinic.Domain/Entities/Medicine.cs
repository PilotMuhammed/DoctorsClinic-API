
namespace DoctorsClinic.Domain.Entities
{
    public class Medicine : BaseEntity<int>
    {
        public required string Name { get; set; }
        public required string Description { get; set; }
        public required string Type { get; set; }

        public ICollection<PrescriptionMedicine>? PrescriptionMedicines { get; set; } 
    }
}
