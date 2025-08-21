using Api.Helper;
using DoctorsClinic.Core.Dtos.Permission;
using DoctorsClinic.Core.Dtos.Role;
using DoctorsClinic.Domain.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


namespace Api.Controllers.Account
{
    [Authorize]
    [AuthorizePermission(Permissions.Users_View)]
    [Route("api/[controller]")]
    [ApiController]
    public class RoleController : ControllerBase
    {

        [HttpGet]
        public IActionResult GetAllRoles()
        {
            var roles = Enum.GetValues(typeof(UserRole))
                .Cast<UserRole>()
                .Select(r => new GetRole
                {
                    Id = (int)r,
                    Name = r.ToString()
                })
                .ToList();

            return Ok(roles);
        }

        [HttpGet("{roleId}/permissions")]
        public IActionResult GetRolePermissions(int roleId)
        {
            if (!Enum.IsDefined(typeof(UserRole), roleId))
                return BadRequest("Invalid role ID.");

            var role = (UserRole)roleId;
            var permissions = RolePermissionMap.GetPermissions(role);

            return Ok(permissions);
        }
    }
}
