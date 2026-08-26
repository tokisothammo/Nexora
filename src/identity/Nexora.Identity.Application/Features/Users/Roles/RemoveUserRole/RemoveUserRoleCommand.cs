namespace Nexora.Identity.Application.Features.Users.Roles.RemoveUserRole;

public sealed class RemoveUserRoleCommand
{
    public Guid UserId { get; init; }

    public string RoleCode { get; init; } = string.Empty;
}