using Asp.Versioning;
using Mapster;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApi.Api.Dtos;
using WebApi.Api.Dtos.Users;
using WebApi.Application.Commands.Users.DeleteUser;
using WebApi.Application.Commands.Users.RegisterUser;
using WebApi.Application.Commands.Users.UpdateUser;
using WebApi.Application.Queries.Users.Dtos;
using WebApi.Application.Queries.Users.ListUsers;

namespace WebApi.Api.Controllers;

/// <summary>
/// Users Controller
/// </summary>
[ApiController]
[Authorize]
[ApiVersion(1.0)]
[Route("api/v{version:apiVersion}/users")]
public class UsersController : ControllerBase
{
    private readonly IMediator _mediator;

    /// <summary>
    /// コンストラクタ
    /// </summary>
    public UsersController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// ユーザー一覧取得
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(IReadOnlyList<UserDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<IReadOnlyList<UserDto>>> ListAllAsync(CancellationToken cancellationToken)
    {
        var query = new ListUsersQuery();
        var result = await _mediator.Send(query, cancellationToken);
        return Ok(result);
    }

    /// <summary>
    /// ユーザー登録
    /// </summary>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> RegisterAsync(
        [FromBody] RegisterUserRequest request, CancellationToken cancellationToken)
    {
        var command = request.Adapt<RegisterUserCommand>();
        await _mediator.Send(command, cancellationToken);
        return Created();
    }

    /// <summary>
    /// ユーザー編集
    /// </summary>
    [HttpPatch("{userId:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateAsync(
        Guid userId, [FromBody] UpdateUserRequest request, CancellationToken cancellationToken)
    {
        var command = request.Adapt<UpdateUserCommand>();
        await _mediator.Send(command, cancellationToken);
        return NoContent();
    }

    /// <summary>
    /// ユーザー削除
    /// </summary>
    [HttpDelete("{userId:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteAsync(
        Guid userId, CancellationToken cancellationToken)
    {
        var command = new DeleteUserCommand(userId);
        await _mediator.Send(command, cancellationToken);
        return NoContent();
    }
}
