namespace Nexora.Identity.Application.Features.Users.Roles.AssignUserRole;

public sealed class AssignUserRoleCommand
{
    public Guid UserId { get; init; }

    public string RoleCode { get; init; } = string.Empty;

    public Guid AssignedBy { get; init; }
}