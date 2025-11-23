using FluentAssertions;
using MediatR;
using Moq;
using WebApi.Application.Abstractions;
using WebApi.Application.Commands.Users.ChangeUserRole;
using WebApi.Application.Common;
using WebApi.Application.Tests.Helpers.Common;
using WebApi.Domain.Abstractions.Repositories;
using WebApi.Domain.Aggregates.ProjectAggregate;
using WebApi.Domain.Aggregates.UserAggregate;
using WebApi.Tests.Helpers.Builders;

namespace WebApi.Application.Tests.Commands.Projects;

public class ChangeUserRoleHandlerTests : BaseCommandHandlerTest
{
    private ChangeUserRoleHandler _handler;
    private readonly Mock<IUserRepository> _userRepository;
    private readonly UserBuilder _userBuilder;

    public ChangeUserRoleHandlerTests()
    {
        _userRepository = new Mock<IUserRepository>();
        _userBuilder = new UserBuilder();

        _handler = new ChangeUserRoleHandler(
            _userRepository.Object,
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
        var user = _userBuilder.Build();

        _userRepository
            .Setup(x => x.GetByIdAsync(user.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(user);

        // Act
        var command = new ChangeUserRoleCommand(user.Id, SystemRole.RoleType.User);
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().Be(Unit.Value);
        UnitOfWork.Verify(x => x.SaveChangesAsync(
            It.IsAny<IDomainEventPublisher>(), It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Fact]
    public async Task 異常系_Userが存在しない場合()
    {
        // Arrange
        var command = new ChangeUserRoleCommand(Guid.NewGuid(), SystemRole.RoleType.User);

        _userRepository
            .Setup(x => x.GetByIdAsync(command.UserId, It.IsAny<CancellationToken>()))
            .ReturnsAsync((User?)null);

        // Act
        var act = async () => await _handler.Handle(command, CancellationToken.None);

        // Assert
        var ex = await act.Should().ThrowAsync<NotFoundException>();
        ex.Which.ErrorCode.Should().Be("APPLICATION.USER_NOT_FOUND");
        UnitOfWork.Verify(x => x.SaveChangesAsync(
            It.IsAny<IDomainEventPublisher>(), It.IsAny<CancellationToken>()),
            Times.Never);
    }
}
