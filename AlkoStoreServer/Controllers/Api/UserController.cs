using AlkoStoreServer.Base;
using AlkoStoreServer.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace AlkoStoreServer.Controllers.Api
{
    [Route("api")]
    [ApiController]
    public class UserController : BaseController
    {
        [HttpPost("user/login")]
        public async Task<IActionResult> Login()
        {
            return null;
        }

        [HttpPost("user/register")]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            return null;
        }
    }
}
