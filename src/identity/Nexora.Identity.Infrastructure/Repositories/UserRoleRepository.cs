using Microsoft.EntityFrameworkCore;
using Nexora.Identity.Domain.Entities;
using Nexora.Identity.Domain.Repositories;
using Nexora.Identity.Infrastructure.Persistence;

namespace Nexora.Identity.Infrastructure.Repositories;

public sealed class UserRoleRepository
    : IUserRoleRepository
{
    private readonly IdentityDbContext _dbContext;

    public UserRoleRepository(
        IdentityDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<IReadOnlyList<string>>
        GetRoleCodesByUserIdAsync(Guid userId)
    {
        return await _dbContext.UserRoles
            .AsNoTracking()
            .Where(userRole => userRole.UserId == userId)
            .Join(
                _dbContext.Roles.AsNoTracking(),
                userRole => userRole.RoleId,
                role => role.Id,
                (_, role) => role.Code)
            .Distinct()
            .OrderBy(roleCode => roleCode)
            .ToListAsync();
    }

    public async Task AssignRoleByCodeAsync(
        Guid userId,
        string roleCode,
        Guid? assignedBy = null)
    {
        var normalizedRoleCode = roleCode
            .Trim()
            .ToUpperInvariant();

        var role = await _dbContext.Roles
            .AsNoTracking()
            .SingleOrDefaultAsync(
                role => role.Code == normalizedRoleCode);

        if (role is null)
        {
            throw new InvalidOperationException(
                $"Role '{normalizedRoleCode}' was not found.");
        }

        var assignmentAlreadyExists =
            await _dbContext.UserRoles.AnyAsync(
                userRole =>
                    userRole.UserId == userId &&
                    userRole.RoleId == role.Id);

        if (assignmentAlreadyExists)
        {
            return;
        }

        var userRole = new UserRole(
            userId,
            role.Id,
            assignedBy);

        await _dbContext.UserRoles.AddAsync(userRole);

        await _dbContext.SaveChangesAsync();
    }
}