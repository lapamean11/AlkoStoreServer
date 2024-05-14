using System;
using System.Threading.Tasks;
using FirebaseAdmin.Auth;
using Microsoft.AspNetCore.Http;

namespace AlkoStoreServer.Middleware
{
    public class FirebaseJwtMiddleware
    {
        private readonly RequestDelegate _next;

        public FirebaseJwtMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                var authHeader = context.Request.Headers["Authorization"].ToString();
                if (authHeader.StartsWith("Bearer ", StringComparison.OrdinalIgnoreCase))
                {
                    var token = authHeader.Substring("Bearer ".Length).Trim();
                    FirebaseToken decodedToken = await FirebaseAuth.DefaultInstance.VerifyIdTokenAsync(token);

                    var lol = decodedToken.ToString();

                    await _next(context); // Token is valid, proceed with the request
                }
                else
                {
                    context.Response.StatusCode = 401;
                }
            }
            catch (FirebaseAuthException ex)
            {
                context.Response.StatusCode = 401;
            }
        }
    }
}
