namespace WebApi.Application.Abstractions.AuthService;

public interface IAuthService
{
    Task<AuthResult> LoginAsync(string email, string password, CancellationToken cancellationToken = default);
}
