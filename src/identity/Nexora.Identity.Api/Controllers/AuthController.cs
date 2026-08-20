using Microsoft.AspNetCore.Mvc;
using Nexora.Identity.Application.Features.Auth.Login;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace Nexora.Identity.Api.Controllers;

[ApiController]
[Route("api/auth")]
public sealed class AuthController : ControllerBase
{
    private readonly LoginHandler _loginHandler;

    public AuthController(LoginHandler loginHandler)
    {
        _loginHandler = loginHandler;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(
    [FromBody] LoginCommand command)
    {
        try
        {
            var result = await _loginHandler.HandleAsync(command);

            return Ok(new
            {
                Message = "Authentication successful.",
                TokenType = "Bearer",
                result.AccessToken,
                result.ExpiresAt
            });
        }
        catch (InvalidOperationException exception)
        {
            return Unauthorized(new
            {
                Message = exception.Message
            });
        }
    }
    [Authorize]
    [HttpGet("me")]
    public IActionResult GetCurrentUser()
    {
        return Ok(new
        {
            UserId = User.FindFirst(
                ClaimTypes.NameIdentifier)?.Value,

            FirstName = User.FindFirst(
                ClaimTypes.GivenName)?.Value,

            LastName = User.FindFirst(
                ClaimTypes.Surname)?.Value,

            PhoneNumber = User.FindFirst(
                ClaimTypes.MobilePhone)?.Value,

            Email = User.FindFirst(
                ClaimTypes.Email)?.Value
        });
    }
}