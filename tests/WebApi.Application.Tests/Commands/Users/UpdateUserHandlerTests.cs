using FluentAssertions;
using MediatR;
using Moq;
using WebApi.Application.Abstractions;
using WebApi.Application.Abstractions.AuthService;
using WebApi.Application.Commands.Users.UpdateUser;
using WebApi.Application.Common;
using WebApi.Application.Tests.Helpers.Common;
using WebApi.Domain.Abstractions.Repositories;
using WebApi.Domain.Aggregates.UserAggregate;
using WebApi.Tests.Helpers.Builders;

namespace WebApi.Application.Tests.Commands.Users;

public class UpdateUserHandlerTests : BaseCommandHandlerTest
{
    private UpdateUserHandler _handler;
    private readonly Mock<IUserRepository> _userRepository;
    private readonly Mock<IPasswordHashService> _passwordHashService;
    private readonly UserBuilder _userBuilder;

    public UpdateUserHandlerTests()
    {
        _userRepository = new Mock<IUserRepository>();
        _passwordHashService = new Mock<IPasswordHashService>();
        _userBuilder = new UserBuilder();

        _handler = new UpdateUserHandler(
            _userRepository.Object,
            _passwordHashService.Object,
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

        _passwordHashService
            .Setup(x => x.Hash(It.IsAny<string>()))
            .Returns((string pwd) => $"hashed_{pwd}");

        // Act
        var command = new UpdateUserCommand(
            userId: user.Id,
            name: "New Name",
            email: "new@example.com",
            password: "newPassword",
            systemRole: SystemRole.RoleType.User
        );
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
        var command = new UpdateUserCommand(
            userId: Guid.NewGuid(),
            name: null,
            email: null,
            password: null,
            systemRole: SystemRole.RoleType.User
        );

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
