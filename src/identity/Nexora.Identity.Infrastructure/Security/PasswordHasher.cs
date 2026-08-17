using Microsoft.AspNetCore.Identity;
using Nexora.Identity.Application.Security;

namespace Nexora.Identity.Infrastructure.Security;

public sealed class PasswordHasher : IPasswordHasher
{
    private readonly PasswordHasher<object> _passwordHasher = new();

    public string Hash(string password)
    {
        return _passwordHasher.HashPassword(
            null!,
            password);
    }

    public bool Verify(
        string passwordHash,
        string providedPassword)
    {
        var result = _passwordHasher.VerifyHashedPassword(
            null!,
            passwordHash,
            providedPassword);

        return result != PasswordVerificationResult.Failed;
    }
}