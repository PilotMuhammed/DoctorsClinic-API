using DoctorsClinic.Domain.Enums;

namespace DoctorsClinic.Domain.Entities
{
    public class UserRole : BaseEntity<int>
    {
        public string Name { get; set; }
        public string NameAr { get; set; }
        public string? Description { get; set; }
        public ICollection<EPermission> Permissions { get; set; } = new List<EPermission>();
    }
}