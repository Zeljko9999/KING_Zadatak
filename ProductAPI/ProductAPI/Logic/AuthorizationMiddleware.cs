using Microsoft.AspNetCore.Http;
using ProductAPI.Logic.Interfaces;
using System.Threading.Tasks;

namespace ProductAPI.Logic
{


    public class AuthorizationMiddleware
    {
        private readonly RequestDelegate _next;

        public AuthorizationMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context, IAuthLogic authLogic)
        {
            var token = context.Session.GetString("AuthToken");

            if (!string.IsNullOrEmpty(token))
            {
                var user = await authLogic.GetUser(token);
                if (user != null)
                {
                    context.Items["User"] = user;
                }
                else
                {
                    context.Session.Remove("AuthToken");
                    context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                    await context.Response.WriteAsync("Unauthorized");
                    return;
                }
            }

            await _next(context);
        }
    }
}
