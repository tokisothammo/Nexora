using Nexora.Identity.Domain.Repositories;

namespace Nexora.Identity.Application.Features.Users.Roles.AssignUserRole;

public sealed class AssignUserRoleHandler
{
    private readonly IUserRepository _userRepository;
    private readonly IUserRoleRepository _userRoleRepository;

    public AssignUserRoleHandler(
        IUserRepository userRepository,
        IUserRoleRepository userRoleRepository)
    {
        _userRepository = userRepository;
        _userRoleRepository = userRoleRepository;
    }

    public async Task HandleAsync(
        AssignUserRoleCommand command)
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

        await _userRoleRepository.AssignRoleByCodeAsync(
            command.UserId,
            command.RoleCode,
            command.AssignedBy);
    }
}