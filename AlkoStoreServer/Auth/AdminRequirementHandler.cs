using AlkoStoreServer.Models;
using AlkoStoreServer.Repositories.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;

namespace AlkoStoreServer.Auth
{
    public class AdminRequirementHandler : AuthorizationHandler<AdminRequirement>
    {
        private readonly IDbRepository<AdminUser> _adminUserRepository;

        public AdminRequirementHandler(IDbRepository<AdminUser> adminUserRepository)
        {
            _adminUserRepository = adminUserRepository;
        }

        protected override async Task<Task> HandleRequirementAsync(
            AuthorizationHandlerContext context, 
            AdminRequirement requirement
        )
        {
            var lol = context.User.FindFirst("Id")?.Value;

            if (context.User.Identity.IsAuthenticated)
            {
                int UserId = Int32.Parse(context.User.FindFirst("Id")?.Value);

                AdminUser User = await _adminUserRepository.GetById(UserId, au => au.Include(e => e.Role));

                if (User.Role.Identifier == "admin")
                {
                    context.Succeed(requirement);
                }
                else 
                {
                    context.Fail();
                }
            }
            else 
            {
                context.Fail();
            }

            return Task.CompletedTask;
        }
    }
}
