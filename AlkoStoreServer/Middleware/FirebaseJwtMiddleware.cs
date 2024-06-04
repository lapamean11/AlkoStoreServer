using System;
using System.Threading.Tasks;
using FirebaseAdmin.Auth;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace AlkoStoreServer.Middleware
{
    public class FirebaseJwtMiddleware
    {
        private readonly RequestDelegate _next;

        private readonly string[] _exceptions = {
            /*"/api/user/register",
            "/api/get/products"*/
        };

        public FirebaseJwtMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                if (Array.Exists(_exceptions, e => e == context.Request.Path.Value))
                {
                    await _next(context);
                    return;
                }

                var authHeader = context.Request.Headers["Authorization"].ToString();

                if (authHeader.StartsWith("Bearer ", StringComparison.OrdinalIgnoreCase))
                {
                    var token = authHeader.Substring("Bearer ".Length).Trim();
                    FirebaseToken decodedToken = await FirebaseAuth.DefaultInstance.VerifyIdTokenAsync(token);

                    var decodedData = decodedToken.Claims;
                    //Dictionary<string, string> userData = JsonConvert.DeserializeObject<Dictionary<string, string>>(decodedData);

                    context.Items["DecodedUserData"] = decodedData;

                    await _next(context); // Token is valid, proceed with the request
                }
                else
                {
                    // await _next(context);
                    context.Response.StatusCode = 401;
                }
            }
            catch (FirebaseAuthException ex)
            {
                // await _next(context);
                context.Response.StatusCode = 401;
            }
        }
    }
}
