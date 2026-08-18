using Nexora.Identity.Application.Security;
using Nexora.Identity.Domain.Entities;
using Nexora.Identity.Domain.Repositories;

namespace Nexora.Identity.Application.Features.Users.Verification.GenerateRegistrationOtp;

public sealed class GenerateRegistrationOtpHandler
{
    private readonly IOtpGenerator _otpGenerator;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IUserVerificationRepository _verificationRepository;

    public GenerateRegistrationOtpHandler(
        IOtpGenerator otpGenerator,
        IPasswordHasher passwordHasher,
        IUserVerificationRepository verificationRepository)
    {
        _otpGenerator = otpGenerator;
        _passwordHasher = passwordHasher;
        _verificationRepository = verificationRepository;
    }

    public async Task<GenerateRegistrationOtpResult> HandleAsync(
        GenerateRegistrationOtpCommand command)
    {
        var otp = _otpGenerator.Generate();

        var otpHash = _passwordHasher.Hash(otp);

        var expiresAt = DateTime.UtcNow.AddMinutes(5);

        var verification = new UserVerification(
            command.UserId,
            "REGISTRATION",
            command.Channel,
            command.Destination,
            otpHash,
            expiresAt);

        await _verificationRepository.AddAsync(verification);

        return new GenerateRegistrationOtpResult(
            verification.Id,
            otp,
            expiresAt);
    }
}