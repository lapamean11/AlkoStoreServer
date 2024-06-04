using AlkoStoreServer.Base;
using AlkoStoreServer.Data;
using AlkoStoreServer.Models;
using AlkoStoreServer.Models.Request;
using AlkoStoreServer.Models.ViewModels;
using AlkoStoreServer.Repositories;
using AlkoStoreServer.Repositories.Interfaces;
using Azure.Core;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Immutable;

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
        public async Task<IActionResult> Register()
        {
            try
            {
                if (HttpContext.Items.TryGetValue("DecodedUserData", out var decodedData))
                {
                    var tokenData = (ImmutableDictionary<string, object>)decodedData;
                    var userEmail = tokenData["email"];

                    bool emailExists = await (
                        await _userRepository.GetContext()).User.AnyAsync(u => u.Email == userEmail
                    );

                    if (emailExists)
                    {
                        return Conflict("Email already exists.");
                    }

                    User user = new User { Email = (string)userEmail };
                    await _userRepository.CreateEntity(user);

                    return Ok("User registered successfully.");
                }

                return BadRequest("User is not registered successfully.");
            }
            catch (Exception ex) 
            {
                return StatusCode(500, "An error occurred while saving the user.");
            }
        }
    }
}
