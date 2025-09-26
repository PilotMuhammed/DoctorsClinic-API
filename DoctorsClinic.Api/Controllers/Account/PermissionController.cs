using Api.Helper;
using DocotorClinic.Api.Controllers;
using DoctorsClinic.Core.Dtos;
using DoctorsClinic.Core.Dtos.Permission;
using DoctorsClinic.Core.IServices.Account;
using DoctorsClinic.Domain.Enums;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Api.Controllers.Account
{
    public class PermissionController : BaseApiController
    {
        private readonly IPermissionService _service;

        public PermissionController(IPermissionService service)
        {
            _service = service;
        }

        [AuthorizePermission(EPermission.SetPermission)]
        [ProducesResponseType(typeof(ResponseDto<List<GetEnum>>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ResponseDto<string>), (int)HttpStatusCode.BadRequest)]
        [HttpPut("SetToUser/{guid}")]
        public async Task<IActionResult> SetToUser(Guid guid, [FromBody] List<int> Permissions)
        {
            var response = await _service.Set(guid, Permissions);
            return response.Error
                ? BadRequest(response)
                : Ok(response);
        }

        [AuthorizePermission(EPermission.Permission)]
        [ProducesResponseType(typeof(ResponseDto<List<GetEnum>>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ResponseDto<string>), (int)HttpStatusCode.BadRequest)]
        [HttpGet("Get/{guid}")]
        public async Task<IActionResult> GetPermission(Guid guid)
        {
            var response = await _service.GetByUserId(guid);
            return response.Error
                ? BadRequest(response)
                : Ok(response);
        }
	
        [AuthorizePermission(EPermission.Permission)]
        [ProducesResponseType(typeof(ResponseDto<List<GetEnum>>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ResponseDto<string>), (int)HttpStatusCode.BadRequest)]
        [HttpGet("[action]")]
        public ActionResult GetAllPermissions()
        {
            var response = _service.GetAll();
            return Ok(response);
        }
    }
}
