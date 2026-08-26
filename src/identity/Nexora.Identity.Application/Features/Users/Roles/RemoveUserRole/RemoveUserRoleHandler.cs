using Nexora.Identity.Domain.Repositories;

namespace Nexora.Identity.Application.Features.Users.Roles.RemoveUserRole;

public sealed class RemoveUserRoleHandler
{
    private readonly IUserRepository _userRepository;
    private readonly IUserRoleRepository _userRoleRepository;

    public RemoveUserRoleHandler(
        IUserRepository userRepository,
        IUserRoleRepository userRoleRepository)
    {
        _userRepository = userRepository;
        _userRoleRepository = userRoleRepository;
    }

    public async Task HandleAsync(
        RemoveUserRoleCommand command)
    {
        if (string.IsNullOrWhiteSpace(command.RoleCode))
        {
            throw new InvalidOperationException(
                "Role code is required.");
        }

        var user = await _userRepository.GetByIdAsync(
            command.UserId);

        if (user is null)
        {
            throw new InvalidOperationException(
                "User was not found.");
        }

        var normalizedRoleCode = command.RoleCode
            .Trim()
            .ToUpperInvariant();

        var assignedRoleCodes =
            await _userRoleRepository
                .GetRoleCodesByUserIdAsync(command.UserId);

        if (!assignedRoleCodes.Contains(
                normalizedRoleCode,
                StringComparer.OrdinalIgnoreCase))
        {
            throw new InvalidOperationException(
                $"The user does not have the '{normalizedRoleCode}' role.");
        }

        if (normalizedRoleCode == "ADMIN")
        {
            var administratorCount =
                await _userRoleRepository
                    .CountUsersInRoleAsync("ADMIN");

            if (administratorCount <= 1)
            {
                throw new InvalidOperationException(
                    "The final ADMIN role cannot be removed.");
            }
        }

        if (assignedRoleCodes.Count == 1)
        {
            throw new InvalidOperationException(
                "A user must retain at least one role.");
        }
        await _userRoleRepository.RemoveRoleByCodeAsync(
            command.UserId,
            normalizedRoleCode);
    }
}