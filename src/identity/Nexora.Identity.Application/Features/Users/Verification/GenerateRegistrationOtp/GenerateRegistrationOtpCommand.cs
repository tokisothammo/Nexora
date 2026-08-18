namespace Nexora.Identity.Application.Features.Users.Verification.GenerateRegistrationOtp;

public sealed class GenerateRegistrationOtpCommand
{
    public Guid UserId { get; set; }

    public string Destination { get; set; } = string.Empty;

    public string Channel { get; set; } = "SMS";
}