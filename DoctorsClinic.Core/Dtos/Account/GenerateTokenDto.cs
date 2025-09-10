using DoctorsClinic.Domain.Entities;

namespace DoctorsClinic.Core.Dtos.Account
{
    public class GenerateTokenDto
    {
        public int Id { get; set; }  
        public User User { get; set; }
        public UserRole Role { get; set; }
        public List<UserPermission>? Permissions { get; set; }
    }
}