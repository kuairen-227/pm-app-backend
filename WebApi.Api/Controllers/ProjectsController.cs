using Asp.Versioning;
using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApi.Api.Dtos.Request.Projects;
using WebApi.Api.Dtos.Response.Common;
using WebApi.Api.Dtos.Response.Projects;
using WebApi.Application.Commands.Projects.ChangeMemberRole;
using WebApi.Application.Commands.Projects.DeleteProject;
using WebApi.Application.Commands.Projects.InviteMember;
using WebApi.Application.Commands.Projects.LaunchProject;
using WebApi.Application.Commands.Projects.RemoveMember;
using WebApi.Application.Commands.Projects.UpdateProject;
using WebApi.Application.Queries.Projects.GetProjectById;
using WebApi.Application.Queries.Projects.ListProjects;

namespace WebApi.Api.Controllers;

/// <summary>
/// Projects Controller
/// </summary>
[ApiController]
[Authorize]
[ApiVersion(1.0)]
[Route("api/v{version:apiVersion}/projects")]
public class ProjectsController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly IMapper _mapper;

    /// <summary>
    /// コンストラクタ
    /// </summary>
    public ProjectsController(IMediator mediator, IMapper mapper)
    {
        _mediator = mediator;
        _mapper = mapper;
    }

    /// <summary>
    /// プロジェクト一覧取得
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(IReadOnlyList<ProjectResponse>), 200)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<IReadOnlyList<ProjectResponse>>> ListAsync(CancellationToken cancellationToken)
    {
        var query = new ListProjectsQuery();
        var dto = await _mediator.Send(query, cancellationToken);
        var response = _mapper.Map<IReadOnlyList<ProjectResponse>>(dto);
        return Ok(response);
    }

    /// <summary>
    /// プロジェクト単体取得
    /// </summary>
    [HttpGet("{projectId:guid}")]
    [ProducesResponseType(typeof(ProjectDetailResponse), 200)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ProjectDetailResponse>> GetByIdAsync(Guid projectId, CancellationToken cancellationToken)
    {
        var query = new GetProjectByIdQuery(projectId);
        var dto = await _mediator.Send(query, cancellationToken);
        var response = _mapper.Map<ProjectDetailResponse>(dto);
        return Ok(response);
    }

    /// <summary>
    /// プロジェクト作成
    /// </summary>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> LaunchAsync(
        [FromBody] LaunchProjectRequest request, CancellationToken cancellationToken)
    {
        var command = _mapper.Map<LaunchProjectCommand>(request);
        var projectId = await _mediator.Send(command, cancellationToken);

        return CreatedAtAction(
            actionName: nameof(GetByIdAsync),
            controllerName: nameof(ProjectsController).Replace("Controller", ""),
            routeValues: new { projectId },
            value: null
        );
    }

    /// <summary>
    /// プロジェクト編集
    /// </summary>
    [HttpPatch("{projectId:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateAsync(
        Guid projectId, [FromBody] UpdateProjectRequest request, CancellationToken cancellationToken)
    {
        var command = _mapper.Map<UpdateProjectCommand>((projectId, request));
        await _mediator.Send(command, cancellationToken);
        return NoContent();
    }

    /// <summary>
    /// プロジェクト削除
    /// </summary>
    [HttpDelete("{projectId:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteAsync(
        Guid projectId, CancellationToken cancellationToken)
    {
        var command = new DeleteProjectCommand(projectId);
        await _mediator.Send(command, cancellationToken);
        return NoContent();
    }

    /// <summary>
    /// プロジェクトメンバー追加
    /// </summary>
    [HttpPost("{projectId:guid}/members")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> InviteMemberAsync(
        Guid projectId, [FromBody] InviteMemberRequest request, CancellationToken cancellationToken)
    {
        var command = _mapper.Map<InviteMemberCommand>((projectId, request));
        await _mediator.Send(command, cancellationToken);
        return NoContent();
    }

    /// <summary>
    /// プロジェクトロール変更
    /// </summary>
    [HttpPatch("{projectId:guid}/members/{userId:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> ChangeMemberRoleAsync(
        Guid projectId, Guid userId, [FromBody] ChangeMemberRoleRequest request, CancellationToken cancellationToken)
    {
        var command = _mapper.Map<ChangeMemberRoleCommand>((projectId, userId, request));
        await _mediator.Send(command, cancellationToken);
        return NoContent();
    }

    /// <summary>
    /// プロジェクトメンバー削除
    /// </summary>
    [HttpDelete("{projectId:guid}/members/{userId:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> RemoveMemberAsync(
        Guid projectId, Guid userId, CancellationToken cancellationToken)
    {
        var command = new RemoveMemberCommand(projectId, userId);
        await _mediator.Send(command, cancellationToken);
        return NoContent();
    }
}
