using AlkoStoreServer.Base;
using Microsoft.AspNetCore.Mvc;

namespace AlkoStoreServer.Controllers.Api
{
    [Route("api")]
    [ApiController]
    public class LoginController : BaseController
    {
        [HttpPost("user/login")]
        public IActionResult Login()
        {
            return null;
        }
    }
}
