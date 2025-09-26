using DocotorClinic.Api.Controllers;
using DoctorsClinic.Core.Dtos;
using DoctorsClinic.Core.Dtos.Account;
using DoctorsClinic.Core.IServices.Account;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Api.Controllers.Account
{
    [ApiExplorerSettings(IgnoreApi = false)]
    public class AuthController : BaseApiController
    {
        private readonly IAuthService _service;
        private readonly IUserAccessorService _userAccessor;

        public AuthController(IAuthService service, IUserAccessorService userAccessor)
        {
            _service = service;
            _userAccessor = userAccessor;
        }
        [AllowAnonymous]
        [Produces("application/json")]
        [ProducesResponseType(typeof(ResponseDto<LoginUserResponse>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ResponseDto<bool>), (int)HttpStatusCode.BadRequest)]
        [HttpPost]
        public async Task<IActionResult> Login([FromBody] LoginUser login)
        {
            var userLogin = await _service.Login(login);
            return userLogin.Error
                ? BadRequest(userLogin)
                : Ok(userLogin);
        }

        [HttpGet("[action]")]
        public ActionResult TestUserAccessor()
        {
            var response = new UserAccessorResponce
            {
                UserId = _userAccessor.UserId,
                UserName = _userAccessor.UserName,
                RoleName = _userAccessor.RoleName,
                Permissions = _userAccessor.Permissions
            };
            return Ok(response);
        }
        public class UserAccessorResponce
        {
            public string UserName { get; set; }
            public Guid UserId { get; set; }
            public string RoleName { get; set; }
            public string Permissions { get; set; }
        }
    }
}
