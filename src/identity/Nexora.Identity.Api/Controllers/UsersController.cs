using Microsoft.AspNetCore.Mvc;
using Nexora.Identity.Application.Features.Users.CreateUser;

namespace Nexora.Identity.Api.Controllers;

[ApiController]
[Route("api/users")]
public class UsersController : ControllerBase
{
    private readonly CreateUserHandler _handler;

    public UsersController(CreateUserHandler handler)
    {
        _handler = handler;
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateUserCommand command)
    {
        await _handler.HandleAsync(command);

        return Ok(new
        {
            Message = "User processed successfully.",
            FirstName = command.FirstName,
            LastName = command.LastName,
            PhoneNumber = command.PhoneNumber,
            Email = command.Email
        });
    }
}