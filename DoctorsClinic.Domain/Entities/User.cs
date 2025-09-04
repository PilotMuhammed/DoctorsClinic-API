using DoctorsClinic.Domain.Enums;

namespace DoctorsClinic.Domain.Entities
{
    public class User : BaseEntity<int>
    {
        public required string Username { get; set; }
        public required string PasswordHash { get; set; }
        public UserRole Role { get; set; }
        public int? DoctorID { get; set; }

        public Doctor? Doctor { get; set; }
    }
}
