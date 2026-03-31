using Asp.Versioning;
using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApi.Api.Dtos.Request.Users;
using WebApi.Api.Dtos.Response.Common;
using WebApi.Api.Dtos.Response.Users;
using WebApi.Application.Commands.Users.DeleteUser;
using WebApi.Application.Commands.Users.RegisterUser;
using WebApi.Application.Commands.Users.UpdateUser;
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
    private readonly IMapper _mapper;

    /// <summary>
    /// コンストラクタ
    /// </summary>
    public UsersController(IMediator mediator, IMapper mapper)
    {
        _mediator = mediator;
        _mapper = mapper;
    }

    /// <summary>
    /// ユーザー一覧取得
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(IReadOnlyList<UserResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<IReadOnlyList<UserResponse>>> ListAllAsync(CancellationToken cancellationToken)
    {
        var query = new ListUsersQuery();
        var dto = await _mediator.Send(query, cancellationToken);
        var response = _mapper.Map<IReadOnlyList<UserResponse>>(dto);
        return Ok(response);
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
        var command = _mapper.Map<RegisterUserCommand>(request);
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
        var command = _mapper.Map<UpdateUserCommand>(request);
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
