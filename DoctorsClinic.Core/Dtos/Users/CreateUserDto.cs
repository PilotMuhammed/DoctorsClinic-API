using DoctorsClinic.Core.Helper;
using DoctorsClinic.Domain.Enums;

namespace DoctorsClinic.Core.Dtos.Users
{
    public class CreateUserDto
    {
        public required string FullName { get; set; }
        public required string UserName { get; set; }
        [PasswordValidation]
        public required string Password { get; set; }
        public Gender? Gender { get; set; }
        public int? UserRoleID { get; set; }
        public List<int>? Permissions { get; set; }
    }
}