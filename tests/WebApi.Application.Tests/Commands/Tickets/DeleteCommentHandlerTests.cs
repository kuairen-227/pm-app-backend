using FluentAssertions;
using Moq;
using WebApi.Application.Commands.Tickets.DeleteComment;
using WebApi.Application.Common;
using WebApi.Application.Tests.Helpers.Common;
using WebApi.Domain.Abstractions.Repositories;
using WebApi.Domain.Aggregates.TicketAggregate;
using WebApi.Tests.Helpers.Builders;

namespace WebApi.Application.Tests.Commands.Tickets;

public class DeleteCommentHandlerTests : BaseCommandHandlerTest
{
    private readonly DeleteCommentHandler _handler;
    private readonly Mock<ITicketRepository> _ticketRepository;
    private readonly TicketBuilder _ticketBuilder;
    private readonly TicketCommentBuilder _ticketCommentBuilder;

    public DeleteCommentHandlerTests()
    {
        _ticketRepository = new Mock<ITicketRepository>();
        _ticketBuilder = new TicketBuilder();
        _ticketCommentBuilder = new TicketCommentBuilder();

        _handler = new DeleteCommentHandler(
            _ticketRepository.Object,
            UnitOfWork.Object,
            DomainEventPublisher.Object,
            UserContext.Object,
            Clock.Object
        );
    }

    [Fact]
    public async Task 正常系_Handle()
    {
        // Arrange
        var ticket = _ticketBuilder
            .WithComments(_ticketCommentBuilder.WithAuthorId(UserContext.Object.Id).Build())
            .Build();
        _ticketRepository.Setup(x => x.GetByIdAsync(ticket.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(ticket);

        // Act
        var command = new DeleteCommentCommand(ticket.ProjectId, ticket.Id, ticket.Comments[0].Id);
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().Be(MediatR.Unit.Value);
        UnitOfWork.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task 異常系_Handle_Ticketが存在しない場合()
    {
        // Arrange
        var command = new DeleteCommentCommand(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid());
        _ticketRepository.Setup(x => x.GetByIdAsync(command.TicketId, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Ticket?)null);

        // Act
        var act = async () => await _handler.Handle(command, CancellationToken.None);

        // Assert
        var ex = await act.Should().ThrowAsync<NotFoundException>();
        ex.Which.ErrorCode.Should().Be("TICKET_NOT_FOUND");
        UnitOfWork.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
    }
}
