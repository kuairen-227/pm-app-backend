using System.Security.Cryptography;
using WebApi.Application.Abstractions;

namespace WebApi.Infrastructure.Services;

public class TokenService : ITokenService
{
    public string GenerateSecureToken(int size = 32)
    {
        var randomNumber = new byte[size];
        var rng = RandomNumberGenerator.Create();
        rng.GetBytes(randomNumber);
        return Convert.ToBase64String(randomNumber);
    }
}
