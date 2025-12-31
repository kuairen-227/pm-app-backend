using FluentAssertions;
using Moq;
using WebApi.Application.Queries.Tickets;
using WebApi.Application.Queries.Tickets.Dtos;
using WebApi.Application.Queries.Tickets.ListProjectTickets;
using WebApi.Application.Tests.Helpers.Common;
using WebApi.Domain.Abstractions;
using WebApi.Domain.Abstractions.Repositories;
using WebApi.Domain.Aggregates.TicketAggregate;
using WebApi.Domain.Common;
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
                    CreatedBy = t.AuditInfo.CreatedBy,
                    CreatedAt = t.AuditInfo.CreatedAt,
                    UpdatedBy = t.AuditInfo.UpdatedBy,
                    UpdatedAt = t.AuditInfo.UpdatedAt
                }).ToList());

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

        _ticketRepository
            .Setup(r => r.ListByProjectIdAsync(
                project.Id,
                It.IsAny<ISpecification<Ticket>>(),
                0, 20, "UpdatedAt", SortOrder.Desc,
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(new PagedResult<Ticket>(tickets, tickets.Count));

        // Act
        var query = new ListProjectTicketsQuery(project.Id);
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.TotalCount.Should().Be(tickets.Count);

        for (int i = 0; i < tickets.Count; i++)
        {
            result.Items.ElementAt(i).Id.Should().Be(tickets[i].Id);
            result.Items.ElementAt(i).Title.Should().Be(tickets[i].Title.Value);
            result.Items.ElementAt(i).AssigneeId.Should().Be(tickets[i].AssigneeId);
            result.Items.ElementAt(i).StartDate.Should().Be(tickets[i].Schedule.StartDate);
            result.Items.ElementAt(i).EndDate.Should().Be(tickets[i].Schedule.EndDate);
            result.Items.ElementAt(i).Status.Should().Be(tickets[i].Status.Value.ToString());
        }

        _ticketRepository.Verify(r => r.ListByProjectIdAsync(
            project.Id,
            It.IsAny<ISpecification<Ticket>>(),
            0, 20, "UpdatedAt", SortOrder.Desc,
            It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task 正常系_Handle_Filterが単独条件の場合()
    {
        // Arrange
        var project = _projectBuilder.Build();
        var tickets = new List<Ticket>
        {
            _ticketBuilder.WithTitle("チケット1").WithProjectId(project.Id).Build(),
            _ticketBuilder.WithTitle("チケット2").WithProjectId(project.Id).Build(),
        };

        _ticketRepository
            .Setup(r => r.ListByProjectIdAsync(
                project.Id,
                It.IsAny<ISpecification<Ticket>>(),
                0, 20, "UpdatedAt", SortOrder.Desc,
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(new PagedResult<Ticket>(tickets, tickets.Count));

        // Act
        var filter = new TicketFilter
        {
            Title = "チケット1"
        };
        var query = new ListProjectTicketsQuery(project.Id) { Filter = filter };
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        _ticketRepository.Verify(r => r.ListByProjectIdAsync(
            project.Id,
            It.Is<ISpecification<Ticket>>(spec =>
                spec.ToExpression().Compile().Invoke(tickets[0]) &&
                !spec.ToExpression().Compile().Invoke(tickets[1])
            ),
            0, 20, "UpdatedAt", SortOrder.Desc,
            It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task 正常系_Handle_Filterが複合条件()
    {
        // Arrange
        var project = _projectBuilder.Build();
        var tickets = new List<Ticket>
        {
            _ticketBuilder.WithTitle("チケット1").WithProjectId(project.Id).WithStatus(TicketStatus.StatusType.Todo).Build(),
            _ticketBuilder.WithTitle("チケット1").WithProjectId(project.Id).WithStatus(TicketStatus.StatusType.InProgress).Build(),
            _ticketBuilder.WithTitle("チケット2").WithProjectId(project.Id).WithStatus(TicketStatus.StatusType.Todo).Build(),
        };

        _ticketRepository
            .Setup(r => r.ListByProjectIdAsync(
                project.Id,
                It.IsAny<ISpecification<Ticket>>(),
                0, 20, "UpdatedAt", SortOrder.Desc,
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(new PagedResult<Ticket>(tickets, tickets.Count));

        // Act
        var filter = new TicketFilter
        {
            Title = "チケット1",
            Status = TicketStatus.StatusType.Todo.ToString()
        };
        var query = new ListProjectTicketsQuery(project.Id) { Filter = filter };
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        _ticketRepository.Verify(r => r.ListByProjectIdAsync(
            project.Id,
            It.Is<ISpecification<Ticket>>(spec =>
                spec.ToExpression().Compile().Invoke(tickets[0]) &&
                !spec.ToExpression().Compile().Invoke(tickets[1]) &&
                !spec.ToExpression().Compile().Invoke(tickets[2])
            ),
            0, 20, "UpdatedAt", SortOrder.Desc,
            It.IsAny<CancellationToken>()), Times.Once);
    }
}
