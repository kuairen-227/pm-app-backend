using FluentAssertions;
using MediatR;
using Moq;
using WebApi.Application.Commands.Tickets.ChangeStatus;
using WebApi.Application.Common;
using WebApi.Application.Tests.Helpers.Common;
using WebApi.Domain.Abstractions.Repositories;
using WebApi.Domain.Aggregates.TicketAggregate;
using WebApi.Tests.Helpers.Builders;

namespace WebApi.Application.Tests.Commands.Tickets;

public class ChangeStatusHandlerTests : BaseCommandHandlerTest
{
    private readonly ChangeStatusHandler _handler;
    private readonly Mock<ITicketRepository> _ticketRepository;
    private readonly TicketBuilder _ticketBuilder;

    public ChangeStatusHandlerTests()
    {
        _ticketRepository = new Mock<ITicketRepository>();
        _ticketBuilder = new TicketBuilder();

        _handler = new ChangeStatusHandler(
            _ticketRepository.Object,
            UnitOfWork.Object,
            UserContext.Object,
            Clock.Object
        );
    }

    [Fact]
    public async Task 正常系_Handle()
    {
        // Arrange
        var ticket = _ticketBuilder.Build();
        var command = new ChangeStatusCommand(
            ticket.ProjectId,
            ticket.Id,
            TicketStatus.StatusType.Todo
        );

        _ticketRepository
            .Setup(x => x.GetByIdAsync(ticket.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(ticket);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().Be(Unit.Value);
        ticket.Status.Value.Should().Be(command.Status);
        ticket.UpdatedBy.Should().Be(UserContext.Object.Id);
        ticket.UpdatedAt.Should().Be(Clock.Object.Now);

        UnitOfWork.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task 異常系_Handle_Ticketが存在しない場合()
    {
        // Arrange
        var command = new ChangeStatusCommand(
            Guid.NewGuid(),
            Guid.NewGuid(),
            TicketStatus.StatusType.Todo
        );
        _ticketRepository
            .Setup(x => x.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((Ticket?)null);

        // Act
        var act = async () => await _handler.Handle(command, CancellationToken.None);

        // Assert
        var ex = await act.Should().ThrowAsync<NotFoundException>();
        ex.Which.ErrorCode.Should().Be("TICKET_NOT_FOUND");
        UnitOfWork.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
    }
}
