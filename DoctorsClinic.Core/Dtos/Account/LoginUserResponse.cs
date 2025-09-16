using DoctorsClinic.Core.Dtos.Permission;
using DoctorsClinic.Core.Dtos.Role;

namespace DoctorsClinic.Core.Dtos.Account
{
    public class LoginUserResponse
    {
        public int Id { get; set; }
        public string? FullName { get; set; }
        public string? UserName { get; set; } 
        public required string Token { get; set; }                
        public GetRole? Role { get; set; } 
        public List<GetEnum>? Permissions { get; set; }
    }
}