using MediatR;
using WebApi.Application.Abstractions.AuthService;
using WebApi.Application.Commands.Auth.GenerateRefreshToken;
using WebApi.Application.Common;
using WebApi.Domain.Abstractions.Repositories;

namespace WebApi.Application.Services;

public class AuthService : IAuthService
{
    private readonly IMediator _mediator;
    private readonly IUserRepository _userRepository;
    private readonly IPasswordHashService _passwordHashService;
    private readonly IJwtService _jwtService;

    public AuthService(
        IMediator mediator,
        IUserRepository userRepository,
        IPasswordHashService passwordHashService,
        IJwtService jwtService)
    {
        _mediator = mediator;
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

        var accessToken = _jwtService.GenerateAccessToken(user.Id);
        var refreshToken = await _mediator.Send(
            new GenerateRefreshTokenCommand(), cancellationToken);

        return new AuthResult(user.Id, accessToken, refreshToken.RefreshToken);
    }
}
