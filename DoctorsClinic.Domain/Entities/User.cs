using DoctorsClinic.Domain.Enums;

namespace DoctorsClinic.Domain.Entities
{
    public class User : BaseEntity<int>
    {
        public string Username { get; set; }
        public string PasswordHash { get; set; }
        public UserRole Role { get; set; }
        public int? DoctorID { get; set; }

        public Doctor? Doctor { get; set; }
    }
}
