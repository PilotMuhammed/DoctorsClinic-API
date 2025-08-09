using DoctorsClinic.Domain.Entities;
using DoctorsClinic.Core.Dtos.Users;
using DoctorsClinic.Core.Dtos.Doctors; 
using DoctorsClinic.Domain.Enums; 
using Mapster;

namespace DoctorsClinic.Core.Mapster
{
    public class UserMap
    {
        public static void Configure()
        {

            TypeAdapterConfig<User, UserDto>.NewConfig()
                .Map(d => d.Role, s => s.Role.ToString())
                .Map(d => d.DoctorName, s => s.Doctor != null ? s.Doctor.FullName : null);

            TypeAdapterConfig<UserDto, User>.NewConfig()
                .Map(d => d.Role, s => ParseRole(s.Role))
                .Ignore(d => d.Doctor)
                .Ignore(d => d.PasswordHash); 

            TypeAdapterConfig<CreateUserDto, User>.NewConfig()
                .Map(d => d.Role, s => ParseRole(s.Role))
                .Map(d => d.PasswordHash, s => s.Password) 
                .Ignore(d => d.UserID)
                .Ignore(d => d.Doctor);

            TypeAdapterConfig<UpdateUserDto, User>.NewConfig()
                .IgnoreIf((s, _) => string.IsNullOrWhiteSpace(s.Username), d => d.Username)
                .IgnoreIf((s, _) => string.IsNullOrWhiteSpace(s.Password), d => d.PasswordHash)
                .IgnoreIf((s, _) => string.IsNullOrWhiteSpace(s.Role), d => d.Role)
                .IgnoreIf((s, _) => s.DoctorID == null, d => d.DoctorID)
                .Map(d => d.Role, s => string.IsNullOrEmpty(s.Role) ? default : ParseRole(s.Role))
                .Map(d => d.PasswordHash, s => s.Password) 
                .Ignore(d => d.Doctor);

            TypeAdapterConfig<User, UserResponseDto>.NewConfig()
                .Map(d => d.User, s => s.Adapt<UserDto>()!)
                .Map(d => d.Doctor, _ => (DoctorDto?)null); 
        }

        private static UserRole ParseRole(string? role)
        {
            return Enum.TryParse<UserRole>(role, true, out var value) ? value : default;
        }
    }
}
