using System.Security.Cryptography;
using Nexora.Identity.Application.Security;

namespace Nexora.Identity.Infrastructure.Security;

public sealed class OtpGenerator : IOtpGenerator
{
    public string Generate()
    {
        var number = RandomNumberGenerator.GetInt32(0, 1_000_000);

        return number.ToString("D6");
    }
}