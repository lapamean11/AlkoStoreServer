using AlkoStoreServer.Base;
using AlkoStoreServer.Models;
using AlkoStoreServer.Models.ViewModels;
using AlkoStoreServer.Repositories.Interfaces;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Html;
using AlkoStoreServer.ViewHelpers.Interfaces;

namespace AlkoStoreServer.Controllers
{
    [Route("adminuser")]
    public class UserController : BaseController
    {
        private readonly IDbRepository<AdminUser> _adminUserRepository;

        private readonly IDbRepository<Role> _roleRepository;

        private readonly IHttpContextAccessor _httpContextAccessor;

        private readonly IHtmlRenderer _htmlRenderer;

        public UserController(
            IDbRepository<AdminUser> adminUserRepository,
            IDbRepository<Role> roleRepository,
            IHttpContextAccessor httpContextAccessor,
            IHtmlRenderer htmlRenderer
        ) 
        {
            _adminUserRepository = adminUserRepository;
            _roleRepository = roleRepository;
            _httpContextAccessor = httpContextAccessor;
            _htmlRenderer = htmlRenderer;
        }

        [HttpPost("login")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                IEnumerable<AdminUser> users = await _adminUserRepository.GetWithInclude(au => au.Include(e => e.Role));
                AdminUser user = users.Where(user => user.Username == model.Username).FirstOrDefault();

                if (user == null || !user.VerifyPassword(model.Password))
                    return RedirectToAction("Index", "Home");

                List<Claim> claims = new List<Claim>
                {
                    new Claim("Username", user.Username),
                    new Claim("Id", user.ID.ToString()),
                    new Claim("IsAdmin", (user.Role.Identifier == "admin" ? 1 : 0).ToString())
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

        [HttpGet("list")]
        [Authorize]
        public async Task<IActionResult> AdminUserList()
        {
            List<AdminUser> users = (List<AdminUser>)await _adminUserRepository.GetWithInclude();

            return View("Views/Layouts/ListLayout.cshtml", users);
        }

        [HttpGet("edit/{id}")]
        [Authorize]
        public async Task<IActionResult> AdminUserEdit(int id)
        {
            AdminUser user = await _adminUserRepository.GetById(id,
                u => u.Include(u => u.Role)
            );

            // IHtmlContent htmlResult = _htmlRenderer.RenderEditForm(user);
            IHtmlContent htmlResult = _htmlRenderer.RenderForm(user);
            ViewBag.Model = user;

            return View("Views/Layouts/EditLayout.cshtml", htmlResult);
        }

        [HttpPost("delete/{id}")]
        [Authorize]
        [Authorize(Policy = "AdminAccess")]
        public async Task<IActionResult> DeleteAdminUser(int id)
        {
            try
            {
                await _adminUserRepository.DeleteAsync(id);

                return RedirectToAction("AdminUserList");
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("create")]
        [Authorize]
        [Authorize(Policy = "AdminAccess")]
        public async Task<IActionResult> CreateNewAdminUser()
        {
            AdminUser user = new AdminUser();

            //IHtmlContent htmlResult = _htmlRenderer.RenderCreateForm(user);
            IHtmlContent htmlResult = _htmlRenderer.RenderForm(user);
            ViewBag.Model = user;

            return View("Views/Layouts/CreateLayout.cshtml", htmlResult);
        }

        [HttpPost("create/save")]
        [Authorize]
        [Authorize(Policy = "AdminAccess")]
        public async Task<IActionResult> SaveNewAdminUser(AdminUser user)
        {
            using (var transaction = await (
                await _adminUserRepository.GetContext()
            ).Database.BeginTransactionAsync())
            {
                try
                {
                    user.RoleId = user.Role.ID;
                    user.SetPassword(user.Password);
                    user.CreatedAt = DateTime.Now;
                    user.Role = null;

                    await _adminUserRepository.CreateEntity(user);
                    await transaction.CommitAsync();

                    return RedirectToAction("AdminUserList");
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();

                    return StatusCode(500, "An error occurred while saving the Category.");
                }
            }
        }

        [HttpPost("edit/save/{id}")]
        [Authorize]
        [Authorize(Policy = "AdminAccess")]
        public async Task<IActionResult> EditAdminUserSave(int id, AdminUser user)
        {
            using (var transaction = await (
                await _adminUserRepository.GetContext()
            ).Database.BeginTransactionAsync())
            {
                try
                {
                    AdminUser userToUpdate = await _adminUserRepository.GetById(id,
                        u => u.Include(u => u.Role)
                    );

                    userToUpdate.Username = user.Username;
                    userToUpdate.RoleId = user.Role.ID;
                    userToUpdate.SetPassword(user.Password);

                    await _adminUserRepository.Update(userToUpdate);
                    await transaction.CommitAsync();

                    return RedirectToAction("AdminUserList");
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();

                    return StatusCode(500, "An error occurred while saving the category.");
                }
            }
        }
    }
}
