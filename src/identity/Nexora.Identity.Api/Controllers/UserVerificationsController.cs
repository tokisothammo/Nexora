using Microsoft.AspNetCore.Mvc;
using Nexora.Identity.Application.Features.Users.Verification.GenerateRegistrationOtp;
using Nexora.Identity.Application.Features.Users.Verification.VerifyRegistrationOtp;

namespace Nexora.Identity.Api.Controllers;

[ApiController]
[Route("api/users/{userId:guid}/verifications")]
public sealed class UserVerificationsController : ControllerBase
{
    private readonly GenerateRegistrationOtpHandler _generateOtpHandler;
    private readonly VerifyRegistrationOtpHandler _verifyOtpHandler;
    private readonly IWebHostEnvironment _environment;

    public UserVerificationsController(
        GenerateRegistrationOtpHandler generateOtpHandler,
        VerifyRegistrationOtpHandler verifyOtpHandler,
        IWebHostEnvironment environment)
    {
        _generateOtpHandler = generateOtpHandler;
        _verifyOtpHandler = verifyOtpHandler;
        _environment = environment;
    }

    [HttpPost("registration-otp")]
    public async Task<IActionResult> GenerateRegistrationOtp(
        Guid userId,
        [FromBody] GenerateRegistrationOtpRequest request)
    {
        var command = new GenerateRegistrationOtpCommand
        {
            UserId = userId,
            Destination = request.Destination,
            Channel = request.Channel
        };

        var result = await _generateOtpHandler.HandleAsync(command);

        if (_environment.IsDevelopment())
        {
            return Ok(new
            {
                Message = "Registration OTP generated successfully.",
                result.VerificationId,
                result.Otp,
                result.ExpiresAt
            });
        }

        return Ok(new
        {
            Message = "Registration OTP generated successfully.",
            result.VerificationId,
            result.ExpiresAt
        });
    }

    [HttpPost("registration-otp/verify")]
    public async Task<IActionResult> VerifyRegistrationOtp(
        Guid userId,
        [FromBody] VerifyRegistrationOtpRequest request)
    {
        var command = new VerifyRegistrationOtpCommand
        {
            UserId = userId,
            Otp = request.Otp
        };

        await _verifyOtpHandler.HandleAsync(command);

        return Ok(new
        {
            Message = "Registration OTP verified successfully.",
            Status = "ACTIVE",
            IsVerified = true
        });
    }
}

public sealed class GenerateRegistrationOtpRequest
{
    public string Destination { get; set; } = string.Empty;

    public string Channel { get; set; } = "SMS";
}

public sealed class VerifyRegistrationOtpRequest
{
    public string Otp { get; set; } = string.Empty;
}