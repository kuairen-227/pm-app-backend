using WebApi.Application.Abstractions.AuthService;
using WebApi.Application.Common;
using WebApi.Domain.Abstractions.Repositories;

namespace WebApi.Infrastructure.Services.AuthService;

public class AuthService : IAuthService
{
    private readonly IUserRepository _userRepository;
    private readonly IPasswordHashService _passwordHashService;
    private readonly IJwtService _jwtService;

    public AuthService(
        IUserRepository userRepository,
        IPasswordHashService passwordHashService,
        IJwtService jwtService)
    {
        _userRepository = userRepository;
        _passwordHashService = passwordHashService;
        _jwtService = jwtService;
    }

    public async Task<AuthResult> LoginAsync(string email, string password, CancellationToken cancellationToken = default)
    {
        var user = await _userRepository.GetByEmailAsync(email, cancellationToken)
            ?? throw new AuthenticationException("INVALID_CREDENTIAL", "メールアドレスまたはパスワードが正しくありません。");

        var isValidPassword = _passwordHashService.Verify(user.PasswordHash, password);
        if (!isValidPassword)
        {
            throw new AuthenticationException("INVALID_CREDENTIAL", "メールアドレスまたはパスワードが正しくありません。");
        }

        var token = _jwtService.GenerateAccessToken(user.Id);

        return new AuthResult(user.Id, token);
    }
}
