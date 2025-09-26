using Api.Helper;
using DocotorClinic.Api.Controllers;
using DoctorsClinic.Core.Dtos;
using DoctorsClinic.Core.Dtos.Role;
using DoctorsClinic.Core.Dtos.Users;
using DoctorsClinic.Core.Helper;
using DoctorsClinic.Core.IServices.Account;
using DoctorsClinic.Domain.Enums;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Api.Controllers.Account
{
    [AuthorizePermission(EPermission.Role)]
    public class RoleController : BaseApiController
    {
        private readonly IRoleService _service;
        public RoleController(IRoleService authService)
        {
            _service = authService;
        }

        [Produces("application/json")]
        [ProducesResponseType(typeof(ResponseDto<PaginationDto<GetRole>>), (int)HttpStatusCode.OK)]
        [HttpGet]
        public async Task<ActionResult> GetAll([FromQuery] PaginationQuery paginationQuery, [FromQuery] RoleFilter filter)
        {
            var response = await _service.GetAll(paginationQuery, filter);
            return Ok(response);
        }

        [AuthorizePermission(EPermission.AddRole)]
        [Produces("application/json")]
        [ProducesResponseType(typeof(ResponseDto<GetRole>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ResponseDto<string>), (int)HttpStatusCode.BadRequest)]
        [HttpPost]
        public async Task<ActionResult> Post([FromBody] CreateRole form)
        {
            var response = await _service.Add(form);
            return response.Error
                ? BadRequest(response)
                : Ok(response);
        }

        [Produces("application/json")]
        [ProducesResponseType(typeof(ResponseDto<IEnumerable<ListDto<int>>>), (int)HttpStatusCode.OK)]
        [HttpGet("[action]")]
        public async Task<ActionResult> GetList()
        {
            var response = await _service.GetList();
            return Ok(response);
        }

        [ProducesResponseType(typeof(ResponseDto<GetRole>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ResponseDto<string>), (int)HttpStatusCode.BadRequest)]
        [HttpGet("{id}")]
        public async Task<ActionResult> GetById(int id)
        {
            var response = await _service.GetById(id);
            return response.Error
                ? BadRequest(response)
                : Ok(response);
        }

        [AuthorizePermission(EPermission.EditRole)]
        [ProducesResponseType(typeof(ResponseDto<GetRole>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ResponseDto<string>), (int)HttpStatusCode.BadRequest)]
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] UpdateRole form)
        {
            var response = await _service.Update(id, form);
            return response.Error
                ? BadRequest(response)
                : Ok(response);
        }

        [AuthorizePermission(EPermission.DeleteRole)]
        [ProducesResponseType(typeof(ResponseDto<bool>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ResponseDto<string>), (int)HttpStatusCode.BadRequest)]
        [HttpDelete("{id}")]
        public async Task<ActionResult<GetRole>> Delete(int id)
        {
            var response = await _service.Delete(id);
            return response.Error
                ? BadRequest(response)
                : Ok(response);
        }

        [AuthorizePermission(EPermission.SetRole)]
        [ProducesResponseType(typeof(ResponseDto<UserResponseDto>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ResponseDto<string>), (int)HttpStatusCode.BadRequest)]
        [HttpPut("Set/{userId}/{roleId}")]
        public async Task<IActionResult> SetRole(Guid userId, int roleId)
        {
            var response = await _service.SetRoleToUser(userId, roleId);
            return response.Error
                ? BadRequest(response)
                : Ok(response);
        }
    }
}
