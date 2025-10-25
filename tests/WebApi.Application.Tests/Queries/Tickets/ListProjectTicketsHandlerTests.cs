using FluentAssertions;
using Moq;
using WebApi.Application.Queries.Tickets.Dtos;
using WebApi.Application.Queries.Tickets.ListProjectTickets;
using WebApi.Application.Tests.Helpers.Common;
using WebApi.Domain.Abstractions.Repositories;
using WebApi.Domain.Aggregates.TicketAggregate;
using WebApi.Tests.Helpers.Builders;

namespace WebApi.Application.Tests.Queries.Tickets;

public class ListProjectTicketsHandlerTests : BaseQueryHandlerTest
{
    private readonly ListProjectTicketsHandler _handler;
    private readonly Mock<ITicketRepository> _ticketRepository;
    private readonly TicketBuilder _ticketBuilder;
    private readonly ProjectBuilder _projectBuilder;

    public ListProjectTicketsHandlerTests()
    {
        _ticketRepository = new Mock<ITicketRepository>();
        _ticketBuilder = new TicketBuilder();
        _projectBuilder = new ProjectBuilder();

        Mapper.Setup(m => m.Map<IEnumerable<TicketDto>>(It.IsAny<IEnumerable<Ticket>>()))
            .Returns<IEnumerable<Ticket>>(tickets =>
                tickets.Select(t => new TicketDto
                {
                    Id = t.Id,
                    Title = t.Title.Value,
                    AssigneeId = t.AssigneeId,
                    Deadline = t.Deadline?.Value,
                    Status = t.Status.Value.ToString(),
                    CompletionCriteria = t.CompletionCriteria,
                }));

        _handler = new ListProjectTicketsHandler(
            _ticketRepository.Object,
            Mapper.Object);
    }

    [Fact]
    public async Task 正常系_Handle()
    {
        // Arrange
        var project = _projectBuilder.Build();
        var tickets = new List<Ticket>
        {
            _ticketBuilder.WithTitle("チケット1").WithProjectId(project.Id).Build(),
            _ticketBuilder.WithTitle("チケット2").WithProjectId(project.Id).Build(),
        };

        _ticketRepository.Setup(r => r.ListByProjectIdAsync(project.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(tickets);

        // Act
        var query = new ListProjectTicketsQuery(project.Id);
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().HaveCount(tickets.Count);

        for (int i = 0; i < tickets.Count; i++)
        {
            result.ElementAt(i).Id.Should().Be(tickets[i].Id);
            result.ElementAt(i).Title.Should().Be(tickets[i].Title.Value);
            result.ElementAt(i).AssigneeId.Should().Be(tickets[i].AssigneeId);
            result.ElementAt(i).Deadline.Should().Be(tickets[i].Deadline?.Value);
            result.ElementAt(i).Status.Should().Be(tickets[i].Status.Value.ToString());
            result.ElementAt(i).CompletionCriteria.Should().Be(tickets[i].CompletionCriteria);
        }
    }
}
