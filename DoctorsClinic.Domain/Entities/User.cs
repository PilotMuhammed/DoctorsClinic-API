using DoctorsClinic.Domain.Enums;

namespace DoctorsClinic.Domain.Entities
{
    public class User : BaseEntity<Guid>
    {
        public string FullName { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public Gender Gender { get; set; } = Gender.undefined;
        public int UserRoleID { get; set; }
        public int? DoctorID { get; set; }

        public ICollection<UserPermission>? Permissions { get; set; }
        public AccountStatus AccountStatus { get; set; } = new AccountStatus();
        public UserRole? Role { get; set; }
        public Doctor? Doctor { get; set; }
    }
}