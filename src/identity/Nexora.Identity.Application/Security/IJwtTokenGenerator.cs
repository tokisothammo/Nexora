using Nexora.Identity.Domain.Entities;

namespace Nexora.Identity.Application.Security;

public interface IJwtTokenGenerator
{
    JwtTokenResult Generate(User user);
}

public sealed class JwtTokenResult
{
    public string AccessToken { get; init; } = string.Empty;

    public DateTime ExpiresAt { get; init; }
}