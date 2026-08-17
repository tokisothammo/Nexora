using Nexora.Identity.Application.Security;
using Nexora.Identity.Domain.Entities;
using Nexora.Identity.Domain.Repositories;

namespace Nexora.Identity.Application.Features.Users.CreateUser;

public class CreateUserHandler
{
    private readonly IUserRepository _userRepository;
    private readonly IPasswordHasher _passwordHasher;

    public CreateUserHandler(
        IUserRepository userRepository,
        IPasswordHasher passwordHasher)
    {
        _userRepository = userRepository;
        _passwordHasher = passwordHasher;
    }

    public async Task HandleAsync(CreateUserCommand command)
    {
        var passwordHash = _passwordHasher.Hash(command.Password);

        var user = new User(
            command.FirstName,
            command.LastName,
            command.PhoneNumber,
            command.Email,
            passwordHash);

        await _userRepository.AddAsync(user);
    }
}