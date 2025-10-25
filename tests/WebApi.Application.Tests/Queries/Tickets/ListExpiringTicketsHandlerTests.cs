using FluentAssertions;
using Moq;
using WebApi.Application.Queries.Tickets.Dtos;
using WebApi.Application.Queries.Tickets.ListExpiringTickets;
using WebApi.Application.Tests.Helpers.Common;
using WebApi.Domain.Abstractions.Repositories;
using WebApi.Domain.Aggregates.TicketAggregate;
using WebApi.Tests.Helpers.Builders;

namespace WebApi.Application.Tests.Queries.Tickets;

public class ListExpiringTicketsHandlerTests : BaseQueryHandlerTest
{
    private readonly ListExpiringTicketsHandler _handler;
    private readonly Mock<ITicketRepository> _ticketRepository;
    private readonly TicketBuilder _ticketBuilder;

    public ListExpiringTicketsHandlerTests()
    {
        _ticketRepository = new Mock<ITicketRepository>();
        _ticketBuilder = new TicketBuilder();

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

        _handler = new ListExpiringTicketsHandler(
            _ticketRepository.Object,
            Mapper.Object);
    }

    [Fact]
    public async Task 正常系_Handle()
    {
        // Arrange
        var tickets = new List<Ticket>
        {
            _ticketBuilder.WithTitle("期限切れチケット").WithDeadline(Clock.Today.AddDays(-1)).Build(),
            _ticketBuilder.WithTitle("期限が近付いてるチケット").WithDeadline(Clock.Today.AddDays(7)).Build(),
        };

        _ticketRepository
            .Setup(x => x.ListExpiringTicketsByAssigneeIdAsync(
                UserContext.Object.Id,
                TimeSpan.FromDays(7),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(tickets);

        // Act
        var query = new ListExpiringTicketsQuery(UserContext.Object.Id);
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().HaveCount(2);

        for (int i = 0; i < result.Count(); i++)
        {
            result.ElementAt(i).Id.Should().Be(tickets[i].Id);
            result.ElementAt(i).Title.Should().Be(tickets[i].Title.Value);
            result.ElementAt(i).AssigneeId.Should().Be(tickets[i].AssigneeId);
            result.ElementAt(i).Deadline.Should().Be(tickets[i].Deadline?.Value);
            result.ElementAt(i).Status.Should().Be(tickets[i].Status.Value.ToString());
            result.ElementAt(i).CompletionCriteria.Should().Be(tickets[i].CompletionCriteria);
        }

        _ticketRepository.Verify(x => x.ListExpiringTicketsByAssigneeIdAsync(
            UserContext.Object.Id, TimeSpan.FromDays(7), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Theory]
    [InlineData(-2)]
    [InlineData(-1)]
    [InlineData(7)]
    [InlineData(8)]
    public async Task 正常系_Handle_期間の指定(int days)
    {
        // Arrange
        var dueWithin = TimeSpan.FromDays(days);

        var tickets = new List<Ticket>
        {
            _ticketBuilder.WithTitle("期限切れチケット").WithDeadline(Clock.Today.AddDays(-1)).Build(),
            _ticketBuilder.WithTitle("期限が近付いてるチケット").WithDeadline(Clock.Today.AddDays(7)).Build(),
            _ticketBuilder.WithTitle("期限に余裕のあるチケット").WithDeadline(Clock.Today.AddDays(8)).Build(),
        };

        _ticketRepository
            .Setup(x => x.ListExpiringTicketsByAssigneeIdAsync(
                UserContext.Object.Id, dueWithin, It.IsAny<CancellationToken>()))
            .ReturnsAsync(tickets.Where(t => t.Deadline != null && t.Deadline.Value <= Clock.Today.AddDays(days)).ToList());

        // Act
        var query = new ListExpiringTicketsQuery(UserContext.Object.Id, dueWithin);
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().HaveCount(tickets.Count(t => t.Deadline != null && t.Deadline.Value <= Clock.Today.AddDays(days)));

        for (int i = 0; i < result.Count(); i++)
        {
            result.ElementAt(i).Id.Should().Be(tickets[i].Id);
            result.ElementAt(i).Title.Should().Be(tickets[i].Title.Value);
            result.ElementAt(i).AssigneeId.Should().Be(tickets[i].AssigneeId);
            result.ElementAt(i).Deadline.Should().Be(tickets[i].Deadline?.Value);
            result.ElementAt(i).Status.Should().Be(tickets[i].Status.Value.ToString());
            result.ElementAt(i).CompletionCriteria.Should().Be(tickets[i].CompletionCriteria);
        }

        _ticketRepository.Verify(x => x.ListExpiringTicketsByAssigneeIdAsync(
            UserContext.Object.Id, dueWithin, It.IsAny<CancellationToken>()), Times.Once);
    }
}
