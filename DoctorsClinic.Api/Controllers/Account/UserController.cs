using Api.Helper;
using DocotorClinic.Api.Controllers;
using DoctorsClinic.Core.Dtos;
using DoctorsClinic.Core.Dtos.Account;
using DoctorsClinic.Core.Dtos.Users;
using DoctorsClinic.Core.Helper;
using DoctorsClinic.Core.IServices.Account;
using DoctorsClinic.Domain.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Net;

namespace Api.Controllers.Account
{
    public class UserController : BaseApiController
    {
        private readonly IUserService _service;
        public UserController(IUserService authService)
        {
            _service = authService;
        }

        [AuthorizePermission(EPermission.User)]
        [Produces("application/json")]
        [ProducesResponseType(typeof(ResponseDto<PaginationDto<UserResponseDto>>), (int)HttpStatusCode.OK)]
        [HttpGet]
        public async Task<ActionResult> GetAll([FromQuery] PaginationQuery paginationQuery, [FromQuery] UserFilterDto filter)
        {
            var response = await _service.GetAll(paginationQuery, filter);
            return Ok(response);
        }

        [Produces("application/json")]
        [ProducesResponseType(typeof(ResponseDto<IEnumerable<ListDto<int>>>), (int)HttpStatusCode.OK)]
        [HttpGet("[action]")]
        public async Task<ActionResult> GetList()
        {
            var response = await _service.GetList();
            return Ok(response);
        }

        [ProducesResponseType(typeof(ResponseDto<UserResponseDto>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ResponseDto<string>), (int)HttpStatusCode.BadRequest)]
        [HttpGet("{guid}")]
        public async Task<ActionResult> GetById(Guid guid)
        {
            var response = await _service.GetById(guid);
            return response.Error
                ? BadRequest(response)
                : Ok(response);
        }

        [Produces("application/json")]
        [ProducesResponseType(typeof(ResponseDto<UserDto>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ResponseDto<string>), (int)HttpStatusCode.BadRequest)]
        [HttpGet("[action]")]
        public async Task<ActionResult> GetProfile()
        {
            var response = await _service.GetProfile();
            return response.Error
                ? BadRequest(response)
                : Ok(response);
        }

        [AuthorizePermission(EPermission.AddUser)]
        [Produces("application/json")]
        [ProducesResponseType(typeof(ResponseDto<UserResponseDto>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ResponseDto<string>), (int)HttpStatusCode.BadRequest)]
        [HttpPost]
        public async Task<ActionResult> Post([FromBody] CreateUserDto form)
        {
            var response = await _service.Add(form);
            return response.Error
                ? BadRequest(response)
                : Ok(response);
        }

        [AuthorizePermission(EPermission.EditUser)]
        [ProducesResponseType(typeof(ResponseDto<UserResponseDto>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ResponseDto<string>), (int)HttpStatusCode.BadRequest)]
        [HttpPut("{guid}")]
        public async Task<IActionResult> Put(Guid guid, [FromBody] UpdateUserDto form)
        {
            var response = await _service.Update(guid, form);
            return response.Error
                ? BadRequest(response)
                : Ok(response);
        }

        [AuthorizePermission(EPermission.DeleteUser)]
        [ProducesResponseType(typeof(ResponseDto<bool>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ResponseDto<string>), (int)HttpStatusCode.BadRequest)]
        [HttpDelete("{guid}")]
        public async Task<ActionResult<UserResponseDto>> Delete(Guid guid)
        {
            var response = await _service.Delete(guid);
            return response.Error
                ? BadRequest(response)
                : Ok(response);
        }

        [Authorize]
        [ProducesResponseType(typeof(ResponseDto<bool>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ResponseDto<bool>), (int)HttpStatusCode.BadRequest)]
        [HttpPut("[action]/{guid}")]
        public async Task<IActionResult> ChangePassword(Guid guid, [FromBody] ChangePassword form)
        {
            var response = await _service.ChangePassword(guid, form);
            return response.Error
                ? BadRequest(response)
                : Ok(response);
        }

        [AuthorizePermission(EPermission.ManageUser)]
        [ProducesResponseType(typeof(ResponseDto<bool>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ResponseDto<bool>), (int)HttpStatusCode.BadRequest)]
        [HttpPut("[action]/{guid}")]
        public async Task<IActionResult> ManageAccount(Guid guid, [Required] bool isBlock)
        {
            var response = await _service.ManageAccount(guid, isBlock);
            return response.Error
                ? BadRequest(response)
                : Ok(response);
        }

        [AuthorizePermission(EPermission.ManageUser)]
        [ProducesResponseType(typeof(ResponseDto<bool>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ResponseDto<bool>), (int)HttpStatusCode.BadRequest)]
        [HttpPut("[action]/{guid}")]
        public async Task<IActionResult> SetActive(Guid guid, [Required] bool isActive)
        {
            var response = await _service.SetActive(guid, isActive);
            return response.Error
                ? BadRequest(response)
                : Ok(response);
        }

        [AuthorizePermission(EPermission.User)]
        [ProducesResponseType(typeof(ResponseDto<UsersCounter>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ResponseDto<string>), (int)HttpStatusCode.BadRequest)]
        [HttpGet("[action]")]
        public async Task<ActionResult> GetCounter()
        {
            var response = await _service.GetCounter();
            return response.Error
                ? BadRequest(response)
                : Ok(response);
        }
    }
}
