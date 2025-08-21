using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace DocotorClinic.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public abstract class BaseApiController : ControllerBase
    {
        // Get User Id
        protected int? CurrentUserId =>
            User.Identity?.IsAuthenticated == true
                ? int.TryParse(User.FindFirstValue(ClaimTypes.NameIdentifier), out var id) ? id : (int?)null
                : null;

        // Get User Name
        protected string? CurrentUserName =>
            User.Identity?.IsAuthenticated == true
                ? User.FindFirstValue(ClaimTypes.Name)
                : null;

        // Get Role
        protected string? CurrentUserRole =>
            User.Identity?.IsAuthenticated == true
                ? User.FindFirstValue(ClaimTypes.Role)
                : null;

        // Get Permissions
        protected List<string> CurrentUserPermissions =>
            User.Identity?.IsAuthenticated == true
                ? (User.FindFirst("PermissionsId")?.Value?.Split(',', StringSplitOptions.RemoveEmptyEntries).ToList() ?? new List<string>())
                : new List<string>();
    }
}
