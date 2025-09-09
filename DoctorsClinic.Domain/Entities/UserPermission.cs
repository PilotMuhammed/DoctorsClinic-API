using DoctorsClinic.Domain.Enums;

namespace DoctorsClinic.Domain.Entities
{
    public class UserPermission : BaseEntity<int>
    {
        public int UserID { get; set; }
        public User User { get; set; }
        public EPermission Permission { get; set; }
    }
}