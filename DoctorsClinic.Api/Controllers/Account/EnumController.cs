using Api.Helper;
using DoctorsClinic.Core.Dtos.Permission;
using DoctorsClinic.Core.Helper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers.Account
{
    [Authorize]
    [AuthorizePermission(Permissions.Users_View)]
    [Route("api/[controller]")]
    [ApiController]
    public class EnumController : ControllerBase
    {
        [HttpGet]
        public IActionResult GetAllEnums()
        {
            var enums = EnumHelper.GetAllEnums();
            return Ok(enums);
        }
    }
}
