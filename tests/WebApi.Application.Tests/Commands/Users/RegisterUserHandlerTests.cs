using FluentAssertions;
using Moq;
using WebApi.Application.Abstractions;
using WebApi.Application.Commands.Users.RegisterUser;
using WebApi.Application.Tests.Helpers.Common;
using WebApi.Domain.Abstractions.Repositories;
using WebApi.Domain.Aggregates.UserAggregate;
using WebApi.Tests.Helpers.Builders;

namespace WebApi.Application.Commands.Users;

public class RegisterUserHandlerTests : BaseCommandHandlerTest
{
    private readonly RegisterUserHandler _handler;
    private readonly Mock<IUserRepository> _userRepository;
    private readonly UserBuilder _userBuilder;

    public RegisterUserHandlerTests()
    {
        _userRepository = new Mock<IUserRepository>();
        _userBuilder = new UserBuilder();

        _handler = new RegisterUserHandler(
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
        User? capturedUser = null;
        _userRepository
            .Setup(x => x.AddAsync(It.IsAny<User>(), It.IsAny<CancellationToken>()))
            .Callback<User, CancellationToken>((u, _) => capturedUser = u)
            .Returns(Task.CompletedTask);

        // Act
        var command = new RegisterUserCommand(
            user.Name,
            user.Email.Value,
            user.Role.Value
        );
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().NotBeEmpty();
        capturedUser.Should().NotBeNull();
        capturedUser.Name.Should().Be(user.Name);
        capturedUser.Email.Should().Be(user.Email);
        capturedUser.Role.Should().Be(user.Role);
        capturedUser.AuditInfo.CreatedBy.Should().Be(UserContext.Object.Id);
        capturedUser.AuditInfo.CreatedAt.Should().Be(Clock.Now);
        capturedUser.AuditInfo.UpdatedBy.Should().Be(UserContext.Object.Id);
        capturedUser.AuditInfo.UpdatedAt.Should().Be(Clock.Now);

        _userRepository.Verify(x => x.AddAsync(It.IsAny<User>(), It.IsAny<CancellationToken>()), Times.Once);
        UnitOfWork.Verify(x => x.SaveChangesAsync(
            It.IsAny<IDomainEventPublisher>(), It.IsAny<CancellationToken>()),
            Times.Once);
    }
}
