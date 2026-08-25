using Nexora.Identity.Application.Security;
using Nexora.Identity.Domain.Enums;
using Nexora.Identity.Domain.Repositories;

namespace Nexora.Identity.Application.Features.Auth.Login;

public sealed class LoginHandler
{
    private readonly IUserRepository _userRepository;
    private readonly IUserRoleRepository _userRoleRepository;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IJwtTokenGenerator _jwtTokenGenerator;

    public LoginHandler(
        IUserRepository userRepository,
        IUserRoleRepository userRoleRepository,
        IPasswordHasher passwordHasher,
        IJwtTokenGenerator jwtTokenGenerator)
    {
        _userRepository = userRepository;
        _userRoleRepository = userRoleRepository;
        _passwordHasher = passwordHasher;
        _jwtTokenGenerator = jwtTokenGenerator;
    }

    public async Task<JwtTokenResult> HandleAsync(
        LoginCommand command)
    {
        var phoneNumber = command.PhoneNumber.Trim();

        var user = await _userRepository
            .GetByPhoneNumberAsync(phoneNumber);

        if (user is null)
        {
            throw new InvalidOperationException(
                "Invalid phone number or password.");
        }

        if (user.Status != UserStatus.Active)
        {
            throw new InvalidOperationException(
                "User account is not active.");
        }

        if (!user.IsVerified)
        {
            throw new InvalidOperationException(
                "User account has not been verified.");
        }

        var passwordIsValid = _passwordHasher.Verify(
            user.PasswordHash,
            command.Password);

        if (!passwordIsValid)
        {
            throw new InvalidOperationException(
                "Invalid phone number or password.");
        }

        var roleCodes = await _userRoleRepository
            .GetRoleCodesByUserIdAsync(user.Id);

        return _jwtTokenGenerator.Generate(
            user,
            roleCodes);
    }
}