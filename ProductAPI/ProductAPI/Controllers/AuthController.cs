using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using ProductAPI.Exceptions;
using ProductAPI.Logic.Interfaces;
using ProductAPI.Models;

namespace ProductAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthLogic _authLogic;

        public AuthController(IAuthLogic authLogic)
        {
            _authLogic = authLogic;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] Login loginRequest)
        {
            var authResponse = await _authLogic.Login(loginRequest);
            if (authResponse != null && !string.IsNullOrEmpty(authResponse.Token))
            {
                HttpContext.Session.SetString("AuthToken", authResponse.Token);
                HttpContext.Session.SetString("Username", authResponse.Username);
                return Ok(authResponse);
            }

            return Unauthorized("Invalid login attempt");
        }
    }
}
