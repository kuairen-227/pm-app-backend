using Asp.Versioning;
using Mapster;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApi.Api.Dtos;
using WebApi.Api.Dtos.Tickets;
using WebApi.Application.Commands.Tickets.AddComment;
using WebApi.Application.Commands.Tickets.AddCompletionCriterion;
using WebApi.Application.Commands.Tickets.CompleteCompletionCriterion;
using WebApi.Application.Commands.Tickets.CreateTicket;
using WebApi.Application.Commands.Tickets.DeleteComment;
using WebApi.Application.Commands.Tickets.DeleteCompletionCriterion;
using WebApi.Application.Commands.Tickets.DeleteTicket;
using WebApi.Application.Commands.Tickets.EditComment;
using WebApi.Application.Commands.Tickets.EditCompletionCriterion;
using WebApi.Application.Commands.Tickets.ReopenCompletionCriterion;
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
[Authorize]
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

    /// <summary>
    /// チケット完了条件追加
    /// </summary>
    [HttpPost("{ticketId:guid}/completion-criteria")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> AddCompletionCriterionAsync(
        Guid projectId,
        Guid ticketId,
        AddCompletionCriterionRequest request,
        CancellationToken cancellationToken)
    {
        var command = (projectId, ticketId, request).Adapt<AddCompletionCriterionCommand>();
        await _mediator.Send(command, cancellationToken);
        return NoContent();
    }

    /// <summary>
    /// チケット完了条件編集
    /// </summary>
    [HttpPatch("{ticketId:guid}/completion-criteria/{criterionId:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> EditCompletionCriterionAsync(
        Guid projectId,
        Guid ticketId,
        Guid criterionId,
        EditCompletionCriterionRequest request,
        CancellationToken cancellationToken)
    {
        var command = (projectId, ticketId, criterionId, request)
            .Adapt<EditCompletionCriterionCommand>();
        await _mediator.Send(command, cancellationToken);
        return NoContent();
    }

    /// <summary>
    /// チケット完了条件削除
    /// </summary>
    [HttpDelete("{ticketId:guid}/completion-criteria/{criterionId:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteCompletionCriterionAsync(
        Guid projectId,
        Guid ticketId,
        Guid criterionId,
        CancellationToken cancellationToken)
    {
        var command = new DeleteCompletionCriterionCommand(
            projectId,
            ticketId,
            criterionId
        );
        await _mediator.Send(command, cancellationToken);
        return NoContent();
    }

    /// <summary>
    /// チケット完了条件完了
    /// </summary>
    [HttpPost("{ticketId:guid}/completion-criteria/{criterionId:guid}/complete")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> CompleteCriterionAsync(
        Guid projectId,
        Guid ticketId,
        Guid criterionId,
        CancellationToken cancellationToken)
    {
        var command = new CompleteCompletionCriterionCommand(
            projectId,
            ticketId,
            criterionId
        );
        await _mediator.Send(command, cancellationToken);
        return NoContent();
    }

    /// <summary>
    /// チケット完了条件再オープン
    /// </summary>
    [HttpPost("{ticketId:guid}/completion-criteria/{criterionId:guid}/reopen")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> ReopenCriterionAsync(
        Guid projectId,
        Guid ticketId,
        Guid criterionId,
        CancellationToken cancellationToken)
    {
        var command = new ReopenCompletionCriterionCommand(
            projectId,
            ticketId,
            criterionId
        );
        await _mediator.Send(command, cancellationToken);
        return NoContent();
    }

    /// <summary>
    /// チケットコメント投稿
    /// </summary>
    [HttpPost("{ticketId:guid}/comments")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> AddCommentAsync(
        Guid projectId, Guid ticketId, AddTicketCommentRequest request, CancellationToken cancellationToken)
    {
        var command = (projectId, ticketId, request).Adapt<AddCommentCommand>();
        await _mediator.Send(command, cancellationToken);
        return NoContent();
    }

    /// <summary>
    /// チケットコメント編集
    /// </summary>
    [HttpPatch("{ticketId:guid}/comments/{commentId:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> EditCommentAsync(
        Guid projectId,
        Guid ticketId,
        Guid commentId,
        EditTicketCommentRequest request,
        CancellationToken cancellationToken)
    {
        var command = (projectId, ticketId, commentId, request).Adapt<EditCommentCommand>();
        await _mediator.Send(command, cancellationToken);
        return NoContent();
    }

    /// <summary>
    /// チケットコメント削除
    /// </summary>
    [HttpDelete("{ticketId:guid}/comments/{commentId:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteCommentAsync(
        Guid projectId, Guid ticketId, Guid commentId, CancellationToken cancellationToken)
    {
        var command = new DeleteCommentCommand(projectId, ticketId, commentId);
        await _mediator.Send(command, cancellationToken);
        return NoContent();
    }
}
