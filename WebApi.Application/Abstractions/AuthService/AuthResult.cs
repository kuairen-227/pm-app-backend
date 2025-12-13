namespace WebApi.Application.Abstractions.AuthService;

public record AuthResult(
    Guid UserId,
    string AccessToken,
    string RefreshToken
);
