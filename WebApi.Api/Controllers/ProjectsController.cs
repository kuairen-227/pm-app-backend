using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using WebApi.Api.Dtos;
using WebApi.Api.Dtos.Projects;
using WebApi.Application.Commands.Projects.ChangeMemberRole;
using WebApi.Application.Commands.Projects.DeleteProject;
using WebApi.Application.Commands.Projects.InviteMember;
using WebApi.Application.Commands.Projects.LaunchProject;
using WebApi.Application.Commands.Projects.RemoveMember;
using WebApi.Application.Commands.Projects.UpdateProject;
using WebApi.Application.Common.Mapper;
using WebApi.Application.Queries.Projects.Dtos;
using WebApi.Application.Queries.Projects.GetProjectById;
using WebApi.Application.Queries.Projects.ListProjects;

namespace WebApi.Api.Controllers.Projects;

/// <summary>
/// Projects Controller
/// </summary>
[ApiController]
[ApiVersion(1.0)]
[Route("api/v{version:apiVersion}/projects")]
public class ProjectsController : ControllerBase
{
    private readonly IMediator _mediator;

    /// <summary>
    /// コンストラクタ
    /// </summary>
    public ProjectsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// プロジェクト一覧取得
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(IReadOnlyList<ProjectDto>), 200)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<IReadOnlyList<ProjectDto>>> ListAsync(CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new ListProjectsQuery(), cancellationToken);
        return Ok(result);
    }

    /// <summary>
    /// プロジェクト単体取得
    /// </summary>
    [HttpGet("{projectId:guid}")]
    [ProducesResponseType(typeof(ProjectDetailDto), 200)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ProjectDetailDto>> GetByIdAsync(Guid projectId, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetProjectByIdQuery(projectId), cancellationToken);
        return Ok(result);
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
        [FromBody] LaunchProjectCommand command, CancellationToken cancellationToken)
    {
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
        var command = new UpdateProjectCommand(
            projectId: projectId,
            name: request.Name,
            description: request.Description
        );

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
        await _mediator.Send(new DeleteProjectCommand(projectId), cancellationToken);
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
        var command = new InviteMemberCommand(
            projectId: projectId,
            userId: request.UserId,
            projectRole: ProjectRoleMapper.Map(request.ProjectRole)
        );

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
        var command = new ChangeMemberRoleCommand(
            projectId: projectId,
            userId: userId,
            projectRole: ProjectRoleMapper.Map(request.ProjectRole)
        );

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
        await _mediator.Send(new RemoveMemberCommand(projectId, userId), cancellationToken);
        return NoContent();
    }
}
