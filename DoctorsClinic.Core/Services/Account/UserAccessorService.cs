using DoctorsClinic.Core.IServices.Account;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace DoctorsClinic.Core.Services.Account
{
    public class UserAccessorService : IUserAccessorService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UserAccessorService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }
        public int UserId
        {
            get
            {
                var idClaim = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                return int.TryParse(idClaim, out var id) ? id : 0;
            }
        }
        public string UserName
        {
            get
            {
                return _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.Name)?.Value ?? "";
            }
        }
        public string RoleName
        {
            get
            {
                return _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.Role)?.Value ?? "";
            }
        }
        public string Permissions
        {
            get
            {
                return _httpContextAccessor.HttpContext?.User?.FindFirst("PermissionsId")?.Value ?? "";
            }
        }
    }
}
