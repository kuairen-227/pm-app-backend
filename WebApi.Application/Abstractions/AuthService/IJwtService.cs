namespace WebApi.Application.Abstractions.AuthService;

public interface IJwtService
{
    string GenerateAccessToken(Guid userId);
}
