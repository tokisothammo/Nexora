namespace Nexora.Identity.Application.Features.Auth.Login;

public sealed class LoginCommand
{
    public string PhoneNumber { get; set; } = string.Empty;

    public string Password { get; set; } = string.Empty;
}