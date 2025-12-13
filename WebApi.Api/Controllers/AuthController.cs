using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using WebApi.Api.Dtos;
using WebApi.Api.Dtos.Auth;
using WebApi.Application.Abstractions.AuthService;

namespace WebApi.Api.Controllers;

/// <summary>
/// Auth Controller
/// </summary>
[ApiController]
[ApiVersion(1.0)]
[Route("api/v{version:apiVersion}/auth")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    /// <summary>
    /// コンストラクタ
    /// </summary>
    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    /// <summary>
    /// ログイン
    /// </summary>
    [HttpPost("login")]
    [ProducesResponseType(typeof(LoginResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<LoginResponse>> LoginAsync(
        [FromBody] LoginRequest request, CancellationToken cancellationToken)
    {
        var result = await _authService.LoginAsync(
            request.Email, request.Password, cancellationToken);

        var response = new LoginResponse
        {
            UserId = result.UserId,
            AccessToken = result.AccessToken,
            RefreshToken = result.RefreshToken
        };

        return Ok(response);
    }
}
