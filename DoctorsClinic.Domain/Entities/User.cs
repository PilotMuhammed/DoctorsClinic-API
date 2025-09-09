
namespace DoctorsClinic.Domain.Entities
{
    public class User : BaseEntity<int>
    {
        public string FullName { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public int UserRoleID { get; set; }
        public int? DoctorID { get; set; }

        public ICollection<UserPermission>? Permissions { get; set; }
        public UserRole? Role { get; set; }
        public Doctor? Doctor { get; set; }
    }
}