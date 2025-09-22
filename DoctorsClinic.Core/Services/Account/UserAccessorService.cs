using DoctorsClinic.Core.IServices.Account;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace DoctorsClinic.Core.Services.Account
{
    public class UserAccessorService : IUserAccessorService
    {
        private readonly IHttpContextAccessor _accessor;
        public UserAccessorService(IHttpContextAccessor accessor)
        {
            _accessor = accessor ?? throw new ArgumentNullException(nameof(accessor));
        }

        public Guid UserId
        {
            get
            {
                var mar = _accessor.HttpContext!.User.Claims
                    .Where(f => f.Type == ClaimTypes.NameIdentifier).Select(f => f.Value).FirstOrDefault();
                return string.IsNullOrWhiteSpace(mar) ? Guid.Empty : new Guid(mar);
            }
        }

        public string UserName => _accessor.HttpContext!.User.Claims
            .Where(f => f.Type == ClaimTypes.Name).Select(f => f.Value).FirstOrDefault()!;

        public string RoleName => _accessor.HttpContext!.User.Claims
            .Where(f => f.Type == ClaimTypes.Role).Select(f => f.Value).FirstOrDefault()!;

        public string Permissions => _accessor.HttpContext!.User.Claims
            .Where(f => f.Type == "PermissionsId").Select(f => f.Value).FirstOrDefault()!;
    }
}
