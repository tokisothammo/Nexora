namespace Nexora.Identity.Domain.Repositories;

public interface IUserRoleRepository
{
    Task<IReadOnlyList<string>> GetRoleCodesByUserIdAsync(
        Guid userId);

    Task AssignRoleByCodeAsync(
        Guid userId,
        string roleCode,
        Guid? assignedBy = null);
}
