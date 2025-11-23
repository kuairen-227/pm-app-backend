using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using WebApi.Api.Dtos;
using WebApi.Application.Commands.Users.DeleteUser;
using WebApi.Application.Commands.Users.RegisterUser;

namespace WebApi.Api.Controllers.Users;

/// <summary>
/// User（Command）
/// </summary>
[ApiController]
[ApiVersion(1.0)]
[Route("api/v{version:apiVersion}/users")]
public class UserCommandController : ControllerBase
{
    private readonly IMediator _mediator;

    /// <summary>
    /// コンストラクタ
    /// </summary>
    public UserCommandController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// ユーザー登録
    /// </summary>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> RegisterAsync([FromBody] RegisterUserCommand command, CancellationToken cancellationToken)
    {
        await _mediator.Send(command, cancellationToken);
        return Created();
    }

    // TODO: ロール変更はユーザー編集APIに変更したい

    /// <summary>
    /// ユーザー削除
    /// </summary>
    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteAsync(Guid id, CancellationToken cancellationToken)
    {
        await _mediator.Send(new DeleteUserCommand(id), cancellationToken);
        return NoContent();
    }
}
