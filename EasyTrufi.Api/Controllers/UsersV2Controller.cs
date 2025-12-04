using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EasyTrufi.Api.Controllers
{
    [Route("api/v{version:ApiVersion}/users")]
    [ApiVersion("2.0")]
    [ApiController]
    public class UsersV2Controller : ControllerBase
    {
        [HttpGet]
        public IActionResult Get()
        {
            return Ok(new
            {
                Version = 2.0,
                Message = "Estoy en la version 2"
            });
        }
    }
}
