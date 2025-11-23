using FluentAssertions;
using Moq;
using WebApi.Application.Common;
using WebApi.Application.Queries.Tickets.Dtos;
using WebApi.Application.Queries.Tickets.GetTicketById;
using WebApi.Application.Tests.Helpers.Common;
using WebApi.Domain.Abstractions.Repositories;
using WebApi.Domain.Aggregates.TicketAggregate;
using WebApi.Tests.Helpers.Builders;

namespace WebApi.Application.Tests.Queries.Tickets;

public class GetTicketByIdTests : BaseQueryHandlerTest
{
    private readonly GetTicketByIdHandler _handler;
    private readonly Mock<ITicketRepository> _ticketRepository;
    private readonly TicketBuilder _ticketBuilder;

    public GetTicketByIdTests()
    {
        _ticketRepository = new Mock<ITicketRepository>();
        _ticketBuilder = new TicketBuilder();

        Mapper.Setup(m => m.Map<TicketDetailDto>(It.IsAny<Ticket>()))
            .Returns<Ticket>(t => new TicketDetailDto
            {
                Id = t.Id,
                Title = t.Title.Value,
                AssigneeId = t.AssigneeId,
                Deadline = t.Deadline?.Value,
                Status = t.Status.Value.ToString(),
                CompletionCriteria = t.CompletionCriteria,
                Comments = t.Comments.Select(c => new TicketCommentDto
                {
                    AuthorId = c.AuthorId,
                    Content = c.Content,
                }).ToList(),
                AssignmentHistories = t.AssignmentHistories.Select(a => new AssignmentHistoryDto
                {
                    ChangeType = a.ChangeType.ToString(),
                    AssigneeId = a.AssigneeId,
                    PreviousAssigneeId = a.PreviousAssigneeId,
                    ChangedAt = a.ChangedAt,
                }).ToList()
            });

        _handler = new GetTicketByIdHandler(
            _ticketRepository.Object,
            Mapper.Object);
    }

    [Fact]
    public async Task 正常系_Handle()
    {
        // Arrange
        var ticket = _ticketBuilder.Build();
        _ticketRepository
            .Setup(x => x.GetByIdAsync(ticket.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(ticket);

        // Act
        var query = new GetTicketByIdQuery(ticket.ProjectId, ticket.Id);
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Id.Should().Be(ticket.Id);
        result.Title.Should().Be(ticket.Title.Value);
        result.AssigneeId.Should().Be(ticket.AssigneeId);
        result.Deadline.Should().Be(ticket.Deadline?.Value);
        result.Status.Should().Be(ticket.Status.Value.ToString());
        result.CompletionCriteria.Should().Be(ticket.CompletionCriteria);
        result.Comments.Should().HaveCount(ticket.Comments.Count);
        result.AssignmentHistories.Should().HaveCount(ticket.AssignmentHistories.Count);
    }

    [Fact]
    public async Task 異常系_Handle_Ticketが存在しない場合()
    {
        // Arrange
        var query = new GetTicketByIdQuery(Guid.NewGuid(), Guid.NewGuid());
        _ticketRepository
            .Setup(x => x.GetByIdAsync(query.TicketId, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Ticket?)null);

        // Act
        var act = async () => await _handler.Handle(query, CancellationToken.None);

        // Assert
        var ex = await act.Should().ThrowAsync<NotFoundException>();
        ex.Which.ErrorCode.Should().Be("APPLICATION.TICKET_NOT_FOUND");
        _ticketRepository.Verify(x => x.GetByIdAsync(query.TicketId, It.IsAny<CancellationToken>()), Times.Once);
    }
}
