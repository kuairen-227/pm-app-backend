using FluentAssertions;
using Moq;
using WebApi.Application.Abstractions;
using WebApi.Application.Commands.Tickets.CreateTicket;
using WebApi.Application.Tests.Helpers.Common;
using WebApi.Domain.Abstractions.Repositories;
using WebApi.Domain.Aggregates.TicketAggregate;
using WebApi.Tests.Helpers.Builders;

namespace WebApi.Application.Tests.Commands.Tickets;

public class CreateTicketHandlerTests : BaseCommandHandlerTest
{
    private readonly CreateTicketHandler _handler;
    private readonly Mock<ITicketRepository> _ticketRepository;
    private readonly TicketBuilder _ticketBuilder;

    public CreateTicketHandlerTests()
    {
        _ticketRepository = new Mock<ITicketRepository>();
        _ticketBuilder = new TicketBuilder();

        _handler = new CreateTicketHandler(
            _ticketRepository.Object,
            UnitOfWork.Object,
            DomainEventPublisher.Object,
            UserContext.Object,
            Clock
        );
    }

    [Fact]
    public async Task 正常系_Handle()
    {
        // Arrange
        var ticket = _ticketBuilder.Build();
        Ticket? capturedTicket = null;
        _ticketRepository
            .Setup(x => x.AddAsync(It.IsAny<Ticket>(), It.IsAny<CancellationToken>()))
            .Callback<Ticket, CancellationToken>((t, _) => capturedTicket = t)
            .Returns(Task.CompletedTask);

        // Act
        var command = new CreateTicketCommand(
            ticket.ProjectId,
            ticket.Title.Value,
            ticket.AssigneeId,
            ticket.Deadline?.Value,
            ticket.CompletionCriteria
        );
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().NotBeEmpty();
        capturedTicket.Should().NotBeNull();
        capturedTicket.ProjectId.Should().Be(ticket.ProjectId);
        capturedTicket.Title.Should().Be(ticket.Title);
        capturedTicket.AssigneeId.Should().Be(ticket.AssigneeId);
        capturedTicket.Deadline.Should().Be(ticket.Deadline);
        capturedTicket.Status.Should().Be(ticket.Status);
        capturedTicket.CompletionCriteria.Should().Be(ticket.CompletionCriteria);
        capturedTicket.CreatedBy.Should().Be(UserContext.Object.Id);
        capturedTicket.CreatedAt.Should().Be(Clock.Now);
        capturedTicket.UpdatedBy.Should().Be(UserContext.Object.Id);
        capturedTicket.UpdatedAt.Should().Be(Clock.Now);

        _ticketRepository.Verify(x => x.AddAsync(It.IsAny<Ticket>(), It.IsAny<CancellationToken>()), Times.Once);
        UnitOfWork.Verify(x => x.SaveChangesAsync(
            It.IsAny<IDomainEventPublisher>(), It.IsAny<CancellationToken>()),
            Times.Once);
    }
}
