using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using WebApi.Api.Dtos;
using WebApi.Application.Queries.Projects.Dtos;
using WebApi.Application.Queries.Projects.GetProjectById;
using WebApi.Application.Queries.Projects.ListProjects;

namespace WebApi.Api.Controllers.Projects;

/// <summary>
/// Project（Query）
/// </summary>
[ApiController]
[ApiVersion(1.0)]
[Route("api/v{version:apiVersion}/projects")]
public class ProjectQueryController : ControllerBase
{
    private readonly IMediator _mediator;

    /// <summary>
    /// コンストラクタ
    /// </summary>
    public ProjectQueryController(IMediator mediator)
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
}
