namespace Nexora.Identity.Application.Features.Users.Verification.VerifyRegistrationOtp;

public sealed class VerifyRegistrationOtpCommand
{
    public Guid UserId { get; set; }

    public string Otp { get; set; } = string.Empty;
}