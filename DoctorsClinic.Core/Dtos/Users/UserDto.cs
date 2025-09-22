using DoctorsClinic.Core.Dtos.Role;
using DoctorsClinic.Domain.Enums;

namespace DoctorsClinic.Core.Dtos.Users
{
    public class UserDto
    {
        public Guid Id { get; set; }
        public string? FullName { get; set; }
        public string? UserName { get; set; }
        public Gender? Gender { get; set; }
        public RoleDto? Role { get; set; }
        public DateTime? CreatedAt { get; set; }
    }
}