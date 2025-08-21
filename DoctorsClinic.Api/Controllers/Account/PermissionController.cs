using Api.Helper;
using DoctorsClinic.Core.Dtos.Permission;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers.Account
{
    [Authorize]
    [AuthorizePermission(Permissions.Users_View)]
    [Route("api/[controller]")]
    [ApiController]
    public class PermissionController : ControllerBase
    {
        [HttpGet]
        public IActionResult GetAllPermissions()
        {
            return Ok(Permissions.All);
        }
    }
}
