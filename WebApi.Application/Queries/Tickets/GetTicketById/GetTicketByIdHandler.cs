using MediatR;
using WebApi.Application.Queries.Tickets.Dtos;
using WebApi.Domain.Abstractions.Repositories;

namespace WebApi.Application.Queries.Tickets.GetTicketById;

public class GetTicketByIdHandler : IRequestHandler<GetTicketByIdQuery, TicketDetailDto?>
{
    private readonly ITicketRepository _ticketRepository;

    public GetTicketByIdHandler(ITicketRepository ticketRepository)
    {
        _ticketRepository = ticketRepository;
    }

    public async Task<TicketDetailDto?> Handle(GetTicketByIdQuery request, CancellationToken cancellationToken)
    {
        var ticket = await _ticketRepository.GetByIdAsync(request.Id, cancellationToken);
        if (ticket is null) return null;

        var ticketDto = new TicketDto
        {
            Id = ticket.Id,
            Title = ticket.Title.Value,
            AssigneeId = ticket.AssigneeId,
            Deadline = ticket.Deadline?.Value,
            Status = ticket.Status.Value.ToString(),
            CompletionCriteria = ticket.CompletionCriteria,
        };

        var commentsDto = ticket.Comments
            .Select(c => new TicketCommentDto
            {
                AuthorId = c.AuthorId,
                Content = c.Content
            })
            .ToList();

        var assignmentHistoryDto = ticket.AssignmentHistories
            .Select(a => new AssignmentHistoryDto
            {
                ChangeType = a.ChangeType.ToString(),
                AssigneeId = a.AssigneeId,
                PreviousAssigneeId = a.PreviousAssigneeId,
                ChangedAt = a.ChangedAt,
            })
            .ToList();

        return TicketDetailDto.From(ticketDto, commentsDto, assignmentHistoryDto);
    }
}
