namespace Nexora.Identity.Application.Features.Users.Verification.GenerateRegistrationOtp;

public sealed record GenerateRegistrationOtpResult(
    Guid VerificationId,
    string Otp,
    DateTime ExpiresAt);