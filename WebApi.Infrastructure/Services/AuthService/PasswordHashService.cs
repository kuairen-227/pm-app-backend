using Microsoft.AspNetCore.Identity;
using WebApi.Application.Abstractions.AuthService;

namespace WebApi.Infrastructure.Services.AuthService;

public class PasswordHashService : IPasswordHashService
{
    private readonly PasswordHasher<object?> _hasher = new();

    public string Hash(string password)
    {
        return _hasher.HashPassword(null, password);
    }

    public bool Verify(string hashedPassword, string plainPassword)
    {
        var result = _hasher.VerifyHashedPassword(null, hashedPassword, plainPassword);
        return result == PasswordVerificationResult.Success ||
               result == PasswordVerificationResult.SuccessRehashNeeded;
    }
}
