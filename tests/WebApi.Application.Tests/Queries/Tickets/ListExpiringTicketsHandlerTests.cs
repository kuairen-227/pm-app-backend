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

        Mapper.Setup(m => m.Map<IReadOnlyList<TicketDto>>(It.IsAny<IEnumerable<Ticket>>()))
            .Returns<IEnumerable<Ticket>>(tickets =>
                tickets.Select(t => new TicketDto
                {
                    Id = t.Id,
                    Title = t.Title.Value,
                    AssigneeId = t.AssigneeId,
                    StartDate = t.Schedule.StartDate,
                    EndDate = t.Schedule.EndDate,
                    Status = t.Status.Value.ToString(),
                    CompletionCriteria = t.CompletionCriteria,
                    CreatedBy = t.AuditInfo.CreatedBy,
                    CreatedAt = t.AuditInfo.CreatedAt,
                    UpdatedBy = t.AuditInfo.UpdatedBy,
                    UpdatedAt = t.AuditInfo.UpdatedAt
                }).ToList());

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
            _ticketBuilder.WithTitle("期限切れチケット").WithEndDate(Clock.Today.AddDays(-1)).Build(),
            _ticketBuilder.WithTitle("期限が近付いてるチケット").WithEndDate(Clock.Today.AddDays(7)).Build(),
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
        result.Should().NotBeNull()
            .And.HaveCount(2)
            .And.BeEquivalentTo(
                tickets.Select(t => new TicketDto
                {
                    Id = t.Id,
                    Title = t.Title.Value,
                    AssigneeId = t.AssigneeId,
                    StartDate = t.Schedule.StartDate,
                    EndDate = t.Schedule.EndDate,
                    Status = t.Status.Value.ToString(),
                    CompletionCriteria = t.CompletionCriteria,
                    CreatedBy = t.AuditInfo.CreatedBy,
                    CreatedAt = t.AuditInfo.CreatedAt,
                    UpdatedBy = t.AuditInfo.UpdatedBy,
                    UpdatedAt = t.AuditInfo.UpdatedAt
                }),
                options => options.WithStrictOrdering()
            );

        _ticketRepository.Verify(x =>
            x.ListExpiringTicketsByAssigneeIdAsync(
                UserContext.Object.Id, TimeSpan.FromDays(7), It.IsAny<CancellationToken>()),
            Times.Once);
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
            _ticketBuilder.WithTitle("期限切れチケット").WithEndDate(Clock.Today.AddDays(-1)).Build(),
            _ticketBuilder.WithTitle("期限が近付いてるチケット").WithEndDate(Clock.Today.AddDays(7)).Build(),
            _ticketBuilder.WithTitle("期限に余裕のあるチケット").WithEndDate(Clock.Today.AddDays(8)).Build(),
        };

        var expected = tickets
            .Where(t => t.Schedule.EndDate != null && t.Schedule.EndDate.Value <= Clock.Today.AddDays(days))
            .ToList();

        _ticketRepository
            .Setup(x => x.ListExpiringTicketsByAssigneeIdAsync(
                UserContext.Object.Id, dueWithin, It.IsAny<CancellationToken>()))
            .ReturnsAsync(expected);

        // Act
        var query = new ListExpiringTicketsQuery(UserContext.Object.Id, dueWithin);
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().HaveCount(expected.Count)
            .And.BeEquivalentTo(
                expected.Select(t => new TicketDto
                {
                    Id = t.Id,
                    Title = t.Title.Value,
                    AssigneeId = t.AssigneeId,
                    StartDate = t.Schedule.StartDate,
                    EndDate = t.Schedule.EndDate,
                    Status = t.Status.Value.ToString(),
                    CompletionCriteria = t.CompletionCriteria
                }),
                options => options.WithStrictOrdering()
            );

        _ticketRepository.Verify(x =>
            x.ListExpiringTicketsByAssigneeIdAsync(
                UserContext.Object.Id, dueWithin, It.IsAny<CancellationToken>()),
            Times.Once);
    }
}
