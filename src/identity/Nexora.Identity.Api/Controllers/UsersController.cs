using Microsoft.AspNetCore.Mvc;
using Nexora.Identity.Application.Features.Users.CreateUser;
using Nexora.Identity.Application.Features.Users.GetUsers;

namespace Nexora.Identity.Api.Controllers;

[ApiController]
[Route("api/users")]
public sealed class UsersController : ControllerBase
{
    private readonly CreateUserHandler _createUserHandler;
    private readonly GetUsersHandler _getUsersHandler;

    public UsersController(
        CreateUserHandler createUserHandler,
        GetUsersHandler getUsersHandler)
    {
        _createUserHandler = createUserHandler;
        _getUsersHandler = getUsersHandler;
    }

    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<UserListItem>>> GetAll()
    {
        var users = await _getUsersHandler.HandleAsync();

        return Ok(users);
    }

    [HttpPost]
    public async Task<IActionResult> Create(
        [FromBody] CreateUserCommand command)
    {
        await _createUserHandler.HandleAsync(command);

        return Ok(new
        {
            Message = "User processed successfully.",
            command.FirstName,
            command.LastName,
            command.PhoneNumber,
            command.Email
        });
    }
}