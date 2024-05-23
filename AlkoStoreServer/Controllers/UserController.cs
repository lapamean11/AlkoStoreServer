using AlkoStoreServer.Base;
using AlkoStoreServer.Models;
using AlkoStoreServer.Models.ViewModels;
using AlkoStoreServer.Repositories.Interfaces;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
//using Microsoft.AspNetCore.Http;

namespace AlkoStoreServer.Controllers
{
    [Route("user")]
    public class UserController : BaseController
    {
        private readonly IDbRepository<AdminUser> _adminUserRepository;

        private readonly IDbRepository<Role> _roleRepository;

        private readonly IHttpContextAccessor _httpContextAccessor;

        public UserController(
            IDbRepository<AdminUser> adminUserRepository,
            IDbRepository<Role> roleRepository,
            IHttpContextAccessor httpContextAccessor
        ) 
        {
            _adminUserRepository = adminUserRepository;
            _roleRepository = roleRepository;
            _httpContextAccessor = httpContextAccessor;
        }

        [HttpPost("login")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                IEnumerable<AdminUser> users = await _adminUserRepository.GetWithInclude();
                AdminUser user = users.Where(user => user.Username == model.Username).FirstOrDefault();

                if (user == null || !user.VerifyPassword(model.Password))
                    return RedirectToAction("Index", "Home");

                List<Claim> claims = new List<Claim>
                {
                    new Claim("Username", user.Username),
                    new Claim("Id", user.ID.ToString())
                };

                var claimsIdentity = new ClaimsIdentity(
                    claims,
                    CookieAuthenticationDefaults.AuthenticationScheme
                );

                var authProperties = new AuthenticationProperties
                {
                    ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(30)
                };

                await _httpContextAccessor.HttpContext.SignInAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme,
                    new ClaimsPrincipal(claimsIdentity),
                    authProperties
                );

                return RedirectToAction("Dashboard", "Home");
            }

            return RedirectToAction("Index", "Home");
        }

        [HttpPost("logout")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await _httpContextAccessor.HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Home");
        }

        [HttpPost("register")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(AdminRegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                // var user = new AdminUser { Username = model.Email, Email = model.Email };
                var roles = await _roleRepository.GetWithInclude();

                AdminUser user = new AdminUser
                {
                    Username = "admin",
                    RoleId = roles.Where(item => item.Identifier == "admin").ToList().FirstOrDefault().ID,
                    CreatedAt = DateTime.Now
                };

                user.SetPassword("admin123");

                bool result = await _adminUserRepository.CreateEntity(user);
            }

            return RedirectToAction("Index", "Home");
        }
    }
}
