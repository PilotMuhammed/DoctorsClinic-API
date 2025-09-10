using DoctorsClinic.Domain.Enums;

namespace DoctorsClinic.Core.Dtos.Users
{
    public class UpdateUserDto
    {
        public string? FullName { get; set; }
        public string? UserName { get; set; }
        public Gender? Gender { get; set; }
    }
}