using AlkoStoreServer.Base;
using AlkoStoreServer.Data;
using AlkoStoreServer.Models;
using AlkoStoreServer.Models.Request;
using AlkoStoreServer.Models.ViewModels;
using AlkoStoreServer.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AlkoStoreServer.Controllers.Api
{
    [Route("api")]
    [ApiController]
    public class UserController : BaseController
    {
        private readonly IDbRepository<User> _userRepository;

        public UserController(IDbRepository<User> userRepository) 
        {
            _userRepository = userRepository;
        }

        [HttpPost("user/register")]
        public async Task<IActionResult> Register([FromBody] UserRegisterRequest request)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(request.Email))
                {
                    return BadRequest("Email is required.");
                }

                bool emailExists = await (await _userRepository.GetContext()).User.AnyAsync(u => u.Email == request.Email);

                if (emailExists)
                {
                    return Conflict("Email already exists.");
                }

                User user = new User { Email = request.Email };
                await _userRepository.CreateEntity(user);

                return Ok("User registered successfully.");
            }
            catch (Exception ex) 
            {
                return StatusCode(500, "An error occurred while saving the user.");
            }
        }
    }
}
