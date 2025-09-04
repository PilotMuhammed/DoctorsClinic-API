
namespace DoctorsClinic.Domain.Entities
{
    public class Specialty : BaseEntity<int>
    {
        public required string Name { get; set; }
        public string? Description { get; set; }

        public ICollection<Doctor>? Doctors { get; set; } 
    }
}
