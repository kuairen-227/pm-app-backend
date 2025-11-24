using Asp.Versioning;
using Mapster;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using WebApi.Api.Dtos;
using WebApi.Api.Dtos.Tickets;
using WebApi.Application.Commands.Tickets.CreateTicket;
using WebApi.Application.Commands.Tickets.DeleteTicket;
using WebApi.Application.Commands.Tickets.UpdateTicket;
using WebApi.Application.Common.Pagination;
using WebApi.Application.Queries.Projects.ListProjects;
using WebApi.Application.Queries.Tickets.Dtos;
using WebApi.Application.Queries.Tickets.GetTicketById;

namespace WebApi.Api.Controllers;

/// <summary>
/// Tickets Controller
/// </summary>
[ApiController]
[ApiVersion(1.0)]
[Route("api/v{version:apiVersion}/projects/{projectId:guid}/tickets")]
public class TicketsController : ControllerBase
{
    private readonly IMediator _mediator;

    /// <summary>
    /// コンストラクタ
    /// </summary>
    public TicketsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// チケット一覧取得
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(PagedResultDto<TicketDto>), 200)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status403Forbidden)]
    public async Task<ActionResult<PagedResultDto<TicketDto>>> ListAsync(CancellationToken cancellationToken)
    {
        var query = new ListProjectsQuery();
        var result = await _mediator.Send(query, cancellationToken);
        return Ok(result);
    }

    /// <summary>
    /// チケット詳細取得
    /// </summary>
    [HttpGet("{ticketId:guid}")]
    [ProducesResponseType(typeof(TicketDetailDto), 200)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<TicketDetailDto>> GetByIdAsync(
        Guid projectId, Guid ticketId, CancellationToken cancellationToken)
    {
        var query = new GetTicketByIdQuery(projectId, ticketId);
        var result = await _mediator.Send(query, cancellationToken);
        return Ok(result);
    }

    /// <summary>
    /// チケット作成
    /// </summary>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> CreateAsync(
        Guid projectId, CreateTicketRequest request, CancellationToken cancellationToken)
    {
        var command = (projectId, request).Adapt<CreateTicketCommand>();
        var ticketId = await _mediator.Send(command, cancellationToken);

        return CreatedAtAction(
            actionName: nameof(GetByIdAsync),
            controllerName: nameof(TicketsController).Replace("Controller", ""),
            routeValues: new { projectId, ticketId },
            value: null
        );
    }

    /// <summary>
    /// チケット編集
    /// </summary>
    [HttpPatch("{ticketId:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateAsync(
        Guid projectId, Guid ticketId, UpdateTicketRequest request, CancellationToken cancellationToken)
    {
        var command = (projectId, ticketId, request).Adapt<UpdateTicketCommand>();
        await _mediator.Send(command, cancellationToken);
        return NoContent();
    }

    /// <summary>
    /// チケット削除
    /// </summary>
    [HttpDelete("{ticketId:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteAsync(
        Guid projectId, Guid ticketId, CancellationToken cancellationToken)
    {
        var command = new DeleteTicketCommand(projectId, ticketId);
        await _mediator.Send(command, cancellationToken);
        return NoContent();
    }
}
