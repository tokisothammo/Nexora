using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Nexora.Identity.Application.Features.Users.Roles.AssignUserRole;
using Nexora.Identity.Application.Features.Users.Roles.RemoveUserRole;

namespace Nexora.Identity.Api.Controllers;

[ApiController]
[Authorize(Roles = "ADMIN")]
[Route("api/users/{userId:guid}/roles")]
public sealed class UserRolesController : ControllerBase
{
    private readonly AssignUserRoleHandler _assignUserRoleHandler;
    private readonly RemoveUserRoleHandler _removeUserRoleHandler;

    public UserRolesController(
        AssignUserRoleHandler assignUserRoleHandler,
        RemoveUserRoleHandler removeUserRoleHandler)
    {
        _assignUserRoleHandler = assignUserRoleHandler;
        _removeUserRoleHandler = removeUserRoleHandler;
    }

    [HttpPost]
    public async Task<IActionResult> AssignRole(
        Guid userId,
        [FromBody] AssignUserRoleRequest request)
    {
        var administratorIdValue = User.FindFirst(
            ClaimTypes.NameIdentifier)?.Value;

        if (!Guid.TryParse(
                administratorIdValue,
                out var administratorId))
        {
            return Unauthorized(new
            {
                Message = "Authenticated administrator is invalid."
            });
        }

        try
        {
            var command = new AssignUserRoleCommand
            {
                UserId = userId,
                RoleCode = request.RoleCode,
                AssignedBy = administratorId
            };

            await _assignUserRoleHandler.HandleAsync(command);

            return Ok(new
            {
                Message = "Role assigned successfully.",
                UserId = userId,
                RoleCode = request.RoleCode
                    .Trim()
                    .ToUpperInvariant(),
                AssignedBy = administratorId
            });
        }
        catch (InvalidOperationException exception)
        {
            return BadRequest(new
            {
                Message = exception.Message
            });
        }
    }

    [HttpDelete("{roleCode}")]
    public async Task<IActionResult> RemoveRole(
        Guid userId,
        string roleCode)
    {
        try
        {
            var command = new RemoveUserRoleCommand
            {
                UserId = userId,
                RoleCode = roleCode
            };

            await _removeUserRoleHandler.HandleAsync(command);

            return Ok(new
            {
                Message = "Role removed successfully.",
                UserId = userId,
                RoleCode = roleCode
                    .Trim()
                    .ToUpperInvariant()
            });
        }
        catch (InvalidOperationException exception)
        {
            return BadRequest(new
            {
                Message = exception.Message
            });
        }
    }
}

public sealed class AssignUserRoleRequest
{
    public string RoleCode { get; set; } = string.Empty;
}