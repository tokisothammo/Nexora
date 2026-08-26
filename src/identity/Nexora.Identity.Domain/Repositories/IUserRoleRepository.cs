namespace Nexora.Identity.Domain.Repositories;

public interface IUserRoleRepository
{
    Task<IReadOnlyList<string>> GetRoleCodesByUserIdAsync(
        Guid userId);

    Task AssignRoleByCodeAsync(
        Guid userId,
        string roleCode,
        Guid? assignedBy = null);

    Task RemoveRoleByCodeAsync(
        Guid userId,
        string roleCode);

    Task<int> CountUsersInRoleAsync(
        string roleCode);
}