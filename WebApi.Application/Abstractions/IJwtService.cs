namespace WebApi.Application.Abstractions;

public interface IJwtService
{
    string GenerateAccessToken(Guid userId);
}
