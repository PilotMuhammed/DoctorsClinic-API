using DocotorClinic.Api.Controllers;
using DoctorsClinic.Core.Helper;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers.Account
{
    public class EnumController : BaseApiController
    {
        [Produces("application/json")]
        [HttpGet]
        public IActionResult GetAllEnums()
        {
            var enums = EnumHelper.GetAllEnums();
            return Ok(enums);
        }
    }
}
