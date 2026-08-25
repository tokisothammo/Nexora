namespace Nexora.Identity.Domain.Entities;

public sealed class UserRole
{
    public Guid UserId { get; private set; }

    public Guid RoleId { get; private set; }

    public DateTime AssignedAt { get; private set; }

    public Guid? AssignedBy { get; private set; }

    private UserRole()
    {
    }

    public UserRole(
        Guid userId,
        Guid roleId,
        Guid? assignedBy = null)
    {
        UserId = userId;
        RoleId = roleId;
        AssignedBy = assignedBy;
        AssignedAt = DateTime.UtcNow;
    }
}