using Microsoft.AspNetCore.Authorization;
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

    [Authorize(Roles = "ADMIN")]
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
        try
        {
            var result =
                await _createUserHandler.HandleAsync(command);

            return Ok(new
            {
                Message = "User registered successfully.",
                result.UserId,
                command.FirstName,
                command.LastName,
                command.PhoneNumber,
                command.Email,
                DefaultRole = "CUSTOMER"
            });
        }
        catch (InvalidOperationException exception)
        {
            return Conflict(new
            {
                Message = exception.Message
            });
        }
    }
}