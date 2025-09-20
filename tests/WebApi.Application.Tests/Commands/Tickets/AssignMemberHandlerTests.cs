using FluentAssertions;
using Moq;
using WebApi.Application.Commands.Tickets.AssignMember;
using WebApi.Application.Common;
using WebApi.Application.Tests.Helpers.Common;
using WebApi.Domain.Abstractions.Repositories;
using WebApi.Domain.Aggregates.ProjectAggregate;
using WebApi.Domain.Aggregates.TicketAggregate;
using WebApi.Domain.Aggregates.UserAggregate;
using WebApi.Tests.Helpers.Builders;

namespace WebApi.Application.Tests.Commands.Tickets;

public class AssignMemberHandlerTests : BaseCommandHandlerTest
{
    private readonly AssignMemberHandler _handler;
    private readonly Mock<ITicketRepository> _ticketRepository;
    private readonly Mock<IUserRepository> _userRepository;
    private readonly Mock<IProjectRepository> _projectRepository;
    private readonly TicketBuilder _ticketBuilder;
    private readonly UserBuilder _userBuilder;
    private readonly ProjectBuilder _projectBuilder;

    public AssignMemberHandlerTests()
    {
        _ticketRepository = new Mock<ITicketRepository>();
        _userRepository = new Mock<IUserRepository>();
        _projectRepository = new Mock<IProjectRepository>();
        _ticketBuilder = new TicketBuilder();
        _userBuilder = new UserBuilder();
        _projectBuilder = new ProjectBuilder();

        _handler = new AssignMemberHandler(
            _ticketRepository.Object,
            _userRepository.Object,
            _projectRepository.Object,
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
        var user = _userBuilder.Build();
        var project = _projectBuilder
            .WithMembers(ProjectMember.Create(
                user.Id, ProjectRole.Create(ProjectRole.RoleType.Member)
            ))
            .Build();

        _ticketRepository.Setup(x => x.GetByIdAsync(ticket.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(ticket);
        _userRepository.Setup(x => x.GetByIdAsync(user.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(user);
        _projectRepository.Setup(x => x.GetByIdAsync(project.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(project);

        // Act
        var command = new AssignMemberCommand(project.Id, ticket.Id, user.Id);
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().Be(MediatR.Unit.Value);
        UnitOfWork.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task 異常系_Handle_Ticketが存在しない場合()
    {
        // Arrange
        var user = _userBuilder.Build();
        var project = _projectBuilder.Build();
        var command = new AssignMemberCommand(project.Id, Guid.NewGuid(), user.Id);

        _ticketRepository.Setup(x => x.GetByIdAsync(command.TicketId, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Ticket?)null);

        // Act
        var act = async () => await _handler.Handle(command, CancellationToken.None);

        // Assert
        var ex = await act.Should().ThrowAsync<NotFoundException>();
        ex.Which.ErrorCode.Should().Be("TICKET_NOT_FOUND");
        UnitOfWork.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task 異常系_Handle_Userが存在しない場合()
    {
        // Arrange
        var ticket = _ticketBuilder.Build();
        var project = _projectBuilder.Build();
        var command = new AssignMemberCommand(project.Id, ticket.Id, Guid.NewGuid());

        _ticketRepository.Setup(x => x.GetByIdAsync(command.TicketId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(ticket);
        _userRepository.Setup(x => x.GetByIdAsync(command.TicketId, It.IsAny<CancellationToken>()))
            .ReturnsAsync((User?)null);

        // Act
        var act = async () => await _handler.Handle(command, CancellationToken.None);

        // Assert
        var ex = await act.Should().ThrowAsync<NotFoundException>();
        ex.Which.ErrorCode.Should().Be("USER_NOT_FOUND");
        UnitOfWork.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task 異常系_Handle_Projectが存在しない場合()
    {
        // Arrange
        var ticket = _ticketBuilder.Build();
        var user = _userBuilder.Build();
        var command = new AssignMemberCommand(Guid.NewGuid(), ticket.Id, user.Id);

        _ticketRepository.Setup(x => x.GetByIdAsync(command.TicketId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(ticket);
        _userRepository.Setup(x => x.GetByIdAsync(command.UserId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(user);
        _projectRepository.Setup(x => x.GetByIdAsync(command.ProjectId, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Project?)null);

        // Act
        var act = async () => await _handler.Handle(command, CancellationToken.None);

        // Assert
        var ex = await act.Should().ThrowAsync<NotFoundException>();
        ex.Which.ErrorCode.Should().Be("PROJECT_NOT_FOUND");
        UnitOfWork.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
    }
}
