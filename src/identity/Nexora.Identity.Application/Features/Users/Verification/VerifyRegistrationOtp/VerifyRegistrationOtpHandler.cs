using Nexora.Identity.Application.Security;
using Nexora.Identity.Domain.Repositories;

namespace Nexora.Identity.Application.Features.Users.Verification.VerifyRegistrationOtp;

public sealed class VerifyRegistrationOtpHandler
{
    private readonly IUserRepository _userRepository;
    private readonly IUserVerificationRepository _verificationRepository;
    private readonly IPasswordHasher _passwordHasher;

    public VerifyRegistrationOtpHandler(
        IUserRepository userRepository,
        IUserVerificationRepository verificationRepository,
        IPasswordHasher passwordHasher)
    {
        _userRepository = userRepository;
        _verificationRepository = verificationRepository;
        _passwordHasher = passwordHasher;
    }

    public async Task HandleAsync(VerifyRegistrationOtpCommand command)
    {
        var user = await _userRepository.GetByIdAsync(command.UserId);

        if (user is null)
        {
            throw new InvalidOperationException("User not found.");
        }

        var verification = await _verificationRepository.GetPendingAsync(
            command.UserId,
            "REGISTRATION");

        if (verification is null)
        {
            throw new InvalidOperationException(
                "No pending registration verification was found.");
        }

        if (verification.IsExpired())
        {
            verification.MarkExpired();

            await _verificationRepository.SaveChangesAsync();

            throw new InvalidOperationException(
                "The OTP has expired.");
        }

        if (!verification.CanAttempt())
        {
            throw new InvalidOperationException(
                "The maximum number of OTP attempts has been reached.");
        }

        var isValid = _passwordHasher.Verify(
            verification.CodeHash,
            command.Otp);

        if (!isValid)
        {
            verification.RecordFailedAttempt();

            await _verificationRepository.SaveChangesAsync();

            throw new InvalidOperationException(
                "The OTP is invalid.");
        }

        verification.MarkVerified();

        user.Activate();

        await _verificationRepository.SaveChangesAsync();
        await _userRepository.UpdateAsync(user);
    }
}