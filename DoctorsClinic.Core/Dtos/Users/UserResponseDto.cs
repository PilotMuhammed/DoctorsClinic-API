using DoctorsClinic.Core.Dtos.Account;
using DoctorsClinic.Core.Dtos.Permission;
using DoctorsClinic.Core.Dtos.Role;
using DoctorsClinic.Domain.Enums;

namespace DoctorsClinic.Core.Dtos.Users
{
    public class UserResponseDto
    {
        public int Id { get; set; }
        public string? FullName { get; set; }
        public string? UserName { get; set; }
        public Gender? Gender { get; set; }
        public DateTime? CreatedAt { get; set; }
        public RoleDto? Role { get; set; }
        public GetAccountStatus? AccountStatus { get; set; }
        public List<GetEnum>? Permissions { get; set; }
    }
}