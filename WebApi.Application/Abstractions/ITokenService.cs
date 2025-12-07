namespace WebApi.Application.Abstractions;

public interface ITokenService
{
    string GenerateSecureToken(int size = 32);
}
