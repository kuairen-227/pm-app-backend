using FluentAssertions;
using MediatR;
using Moq;
using WebApi.Application.Abstractions;
using WebApi.Application.Commands.Users.DeleteUser;
using WebApi.Application.Common;
using WebApi.Application.Tests.Helpers.Common;
using WebApi.Domain.Abstractions.Repositories;
using WebApi.Domain.Aggregates.UserAggregate;
using WebApi.Tests.Helpers.Builders;

namespace WebApi.Application.Tests.Commands.Users;

public class DeleteUserHandlerTests : BaseCommandHandlerTest
{
    private readonly DeleteUserHandler _handler;
    private readonly Mock<IUserRepository> _userRepository;
    private readonly UserBuilder _userBuilder;

    public DeleteUserHandlerTests()
    {
        _userRepository = new Mock<IUserRepository>();
        _userBuilder = new UserBuilder();

        _handler = new DeleteUserHandler(
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
        var user = _userBuilder
            .WithCreatedBy(UserContext.Object.Id)
            .WithClock(Clock)
            .Build();
        _userRepository
            .Setup(x => x.GetByIdAsync(user.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(user);

        // Act
        var command = new DeleteUserCommand(user.Id);
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().Be(Unit.Value);
        _userRepository.Verify(x => x.DeleteAsync(user, It.IsAny<CancellationToken>()), Times.Once);
        UnitOfWork.Verify(x => x.SaveChangesAsync(
            It.IsAny<IDomainEventPublisher>(), It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Fact]
    public async Task 異常系_Handle_Userが存在しない場合()
    {
        // Arrange
        var command = new DeleteUserCommand(Guid.NewGuid());
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
