using Nexora.Identity.Domain.Repositories;

namespace Nexora.Identity.Application.Features.Users.GetUsers;

public sealed class GetUsersHandler
{
    private readonly IUserRepository _userRepository;

    public GetUsersHandler(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<IReadOnlyList<UserListItem>> HandleAsync()
    {
        var users = await _userRepository.GetAllAsync();

        return users
            .Select(user => new UserListItem(
                user.Id,
                user.FirstName,
                user.LastName,
                user.PhoneNumber,
                user.Email,
                user.Status.ToString().ToUpperInvariant(),
                user.IsVerified,
                user.CreatedAt))
            .ToList();
    }
}