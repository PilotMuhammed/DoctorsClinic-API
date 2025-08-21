using DoctorsClinic.Core.Dtos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Api.Helper
{

    public class AuthorizePermissionAttribute : Attribute, IAuthorizationFilter
    {
        private readonly string[] _requiredPermissions;

        public AuthorizePermissionAttribute(params string[] requiredPermissions)
        {
            _requiredPermissions = requiredPermissions;
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var httpContext = context.HttpContext;
            var user = httpContext.User;

            if (user?.Identity is not { IsAuthenticated: true })
            {
                context.Result = new JsonResult(new ResponseDto<bool>("Authentication required", true))
                {
                    StatusCode = StatusCodes.Status401Unauthorized
                };
                return;
            }

            var permissionsClaim = user.Claims.FirstOrDefault(c => c.Type == "PermissionsId");
            if (permissionsClaim == null)
            {
                context.Result = new JsonResult(new ResponseDto<bool>("Permissions not found", true))
                {
                    StatusCode = StatusCodes.Status403Forbidden
                };
                return;
            }

            var userPermissions = permissionsClaim.Value
                .Split(',')
                .Where(p => !string.IsNullOrWhiteSpace(p))
                .ToList();

            var hasAllPermissions = _requiredPermissions.All(rp => userPermissions.Contains(rp));

            if (!hasAllPermissions)
            {
                context.Result = new JsonResult(new ResponseDto<bool>("You don't have the required permissions", true))
                {
                    StatusCode = StatusCodes.Status403Forbidden
                };
            }
        }
    }
}
