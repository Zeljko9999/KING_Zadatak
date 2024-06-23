using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ProductAPI.Exceptions;
using ProductAPI.Logic.Interfaces;
using ProductAPI.Models;
using Microsoft.Extensions.Logging;


namespace ProductAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthLogic _authLogic;
        private readonly ILogger<AuthController> _logger;

        public AuthController(IAuthLogic authLogic, ILogger<AuthController> logger)
        {
            _authLogic = authLogic;
            _logger = logger;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] Login loginRequest)
        {
            _logger.LogInformation("Login attempt for username: {Username}", loginRequest.Username);

            var authResponse = await _authLogic.Login(loginRequest);
            if (authResponse != null && !string.IsNullOrEmpty(authResponse.Token))
            {
                HttpContext.Session.SetString("AuthToken", authResponse.Token);
                HttpContext.Session.SetString("Username", authResponse.Username);

                _logger.LogInformation("Login successful for username: {Username}", authResponse.Username);
                return Ok(authResponse);
            }

            _logger.LogWarning("Invalid login attempt for username: {Username}", loginRequest.Username);
            return Unauthorized("Invalid login attempt");
        }
    }
}
