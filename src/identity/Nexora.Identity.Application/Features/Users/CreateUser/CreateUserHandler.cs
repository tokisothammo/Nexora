
using Nexora.Identity.Domain.Entities;
using Nexora.Identity.Domain.Repositories;

namespace Nexora.Identity.Application.Features.Users.CreateUser;

public class CreateUserHandler
{
    private readonly IUserRepository _userRepository;

    public CreateUserHandler(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task HandleAsync(CreateUserCommand command)
    {
        var user = new User(
            command.Username,
            command.Email,
            command.PhoneNumber,
            command.Password);

        await _userRepository.AddAsync(user);
    }
}