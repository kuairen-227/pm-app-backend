using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using WebApi.Api.Dtos;
using WebApi.Application.Queries.Users.Dtos;
using WebApi.Application.Queries.Users.ListUsers;

namespace WebApi.Api.Controllers.Users;

/// <summary>
/// User（Query）
/// </summary>
[ApiController]
[ApiVersion(1.0)]
[Route("api/v{version:apiVersion}/users")]
public class UserQueryController : ControllerBase
{
    private readonly IMediator _mediator;

    /// <summary>
    /// コンストラクタ
    /// </summary>
    public UserQueryController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// ユーザー一覧取得
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(IReadOnlyList<UserDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status403Forbidden)]
    public async Task<ActionResult<IReadOnlyList<UserDto>>> ListAllAsync(CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new ListUsersQuery(), cancellationToken);
        return Ok(result);
    }
}
