using Nexora.Identity.Application.Security;
using Nexora.Identity.Domain.Entities;
using Nexora.Identity.Domain.Repositories;

namespace Nexora.Identity.Application.Features.Users.CreateUser;

public sealed class CreateUserHandler
{
    private readonly IUserRepository _userRepository;
    private readonly IUserRoleRepository _userRoleRepository;
    private readonly IPasswordHasher _passwordHasher;

    public CreateUserHandler(
        IUserRepository userRepository,
        IUserRoleRepository userRoleRepository,
        IPasswordHasher passwordHasher)
    {
        _userRepository = userRepository;
        _userRoleRepository = userRoleRepository;
        _passwordHasher = passwordHasher;
    }

    public async Task<CreateUserResult> HandleAsync(
        CreateUserCommand command)
    {
        var phoneNumber = command.PhoneNumber.Trim();

        var existingPhoneUser =
            await _userRepository.GetByPhoneNumberAsync(phoneNumber);

        if (existingPhoneUser is not null)
        {
            throw new InvalidOperationException(
                "A user with this phone number already exists.");
        }

        if (!string.IsNullOrWhiteSpace(command.Email))
        {
            var existingEmailUser =
                await _userRepository.GetByEmailAsync(
                    command.Email.Trim());

            if (existingEmailUser is not null)
            {
                throw new InvalidOperationException(
                    "A user with this email address already exists.");
            }
        }

        var passwordHash = _passwordHasher.Hash(
            command.Password);

        var user = new User(
        command.FirstName.Trim(),
        command.LastName.Trim(),
        phoneNumber,
        command.Email?.Trim(),
        passwordHash);

        await _userRepository.AddAsync(user);

        await _userRoleRepository.AssignRoleByCodeAsync(
            user.Id,
            "CUSTOMER");

        return new CreateUserResult
        {
            UserId = user.Id
        };
    }
}