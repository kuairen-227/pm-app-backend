using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApi.Api.Dtos;
using WebApi.Api.Dtos.Auth;
using WebApi.Application.Abstractions.AuthService;
using WebApi.Application.Commands.Auth.RefreshAccessToken;
using WebApi.Application.Commands.Auth.RevokeRefreshToken;

namespace WebApi.Api.Controllers;

/// <summary>
/// Auth Controller
/// </summary>
[ApiController]
[ApiVersion(1.0)]
[Route("api/v{version:apiVersion}/auth")]
public class AuthController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly IAuthService _authService;

    /// <summary>
    /// コンストラクタ
    /// </summary>
    public AuthController(IMediator mediator, IAuthService authService)
    {
        _mediator = mediator;
        _authService = authService;
    }

    /// <summary>
    /// ログイン
    /// </summary>
    [HttpPost("login")]
    [ProducesResponseType(typeof(LoginResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<LoginResponse>> LoginAsync(
        [FromBody] LoginRequest request, CancellationToken cancellationToken)
    {
        var result = await _authService.LoginAsync(
            request.Email, request.Password, cancellationToken);

        Response.Cookies.Append("access_token", result.AccessToken, new CookieOptions
        {
            HttpOnly = true,
            Secure = true,
            SameSite = SameSiteMode.Strict,
            Expires = DateTime.UtcNow.AddMinutes(15)
        });

        Response.Cookies.Append("refresh_token", result.RefreshToken, new CookieOptions
        {
            HttpOnly = true,
            Secure = true,
            SameSite = SameSiteMode.Strict,
            Expires = DateTime.UtcNow.AddDays(7)
        });

        var response = new LoginResponse
        {
            UserId = result.UserId,
        };

        return Ok(response);
    }

    /// <summary>
    /// トークンのリフレッシュ
    /// </summary>
    [HttpPost("refresh")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(RefreshResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<RefreshResponse>> RefreshAsync(CancellationToken cancellationToken)
    {
        if (!Request.Cookies.TryGetValue("refresh_token", out var refreshToken))
        {
            return Unauthorized();
        }

        var command = new RefreshAccessTokenCommand(refreshToken);
        var result = await _mediator.Send(command, cancellationToken);

        Response.Cookies.Append("access_token", result.AccessToken, new CookieOptions
        {
            HttpOnly = true,
            Secure = true,
            SameSite = SameSiteMode.Strict,
            Expires = DateTime.UtcNow.AddMinutes(15)
        });

        var response = new RefreshResponse
        {
            UserId = result.UserId
        };

        return Ok(response);
    }

    /// <summary>
    /// ログアウト
    /// </summary>
    [HttpPost("logout")]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> LogoutAsync(CancellationToken cancellationToken)
    {
        if (!Request.Cookies.TryGetValue("refresh_token", out var refreshToken))
        {
            return BadRequest();
        }

        var command = new RevokeRefreshTokenCommand(refreshToken);
        await _mediator.Send(command, cancellationToken);

        Response.Cookies.Delete("access_token");
        Response.Cookies.Delete("refresh_token");

        return NoContent();
    }
}
