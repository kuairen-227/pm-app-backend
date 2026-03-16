using FluentAssertions;
using Moq;
using WebApi.Application.Common;
using WebApi.Application.Queries.Auth.GetCurrentUser;
using WebApi.Application.Queries.Users.Dtos;
using WebApi.Application.Tests.Helpers.Common;
using WebApi.Domain.Abstractions.Repositories;
using WebApi.Domain.Aggregates.UserAggregate;
using WebApi.Tests.Helpers.Builders;

namespace WebApi.Application.Tests.Queries.Auth;

public class GetCurrentUserHandlerTests : BaseQueryHandlerTest
{
    private readonly Mock<IUserRepository> _userRepository;
    private readonly UserBuilder _userBuilder;
    private readonly User _user;
    private readonly GetCurrentUserHandler _handler;

    public GetCurrentUserHandlerTests()
    {
        _userRepository = new Mock<IUserRepository>();
        _userBuilder = new UserBuilder();
        _user = _userBuilder.Build();

        Mapper.Setup(m => m.Map<UserDto>(It.IsAny<User>()))
            .Returns<User>(u => new UserDto
            {
                Id = u.Id,
                Name = u.Name,
                Email = u.Email.Value,
                Role = u.Role.Value.ToString(),
                CreatedBy = u.AuditInfo.CreatedBy,
                CreatedAt = u.AuditInfo.CreatedAt,
                UpdatedBy = u.AuditInfo.UpdatedBy,
                UpdatedAt = u.AuditInfo.UpdatedAt,
            });

        var userContext = TestHelpers.CreateUserContext(_user.Id);
        _handler = new GetCurrentUserHandler(userContext.Object, _userRepository.Object, Mapper.Object);
    }

    [Fact]
    public async Task 正常系_Handle()
    {
        // Arrange
        _userRepository
            .Setup(x => x.GetByIdAsync(_user.Id, It.IsAny<CancellationToken>()))
                .ReturnsAsync(_user);

        // Act
        var query = new GetCurrentUserQuery();
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.Id.Should().Be(_user.Id);
        result.Name.Should().Be(_user.Name);
        result.Email.Should().Be(_user.Email.Value);
        result.Role.Should().Be(_user.Role.Value.ToString());
        result.CreatedBy.Should().Be(_user.AuditInfo.CreatedBy);
        result.CreatedAt.Should().Be(_user.AuditInfo.CreatedAt);
        result.UpdatedBy.Should().Be(_user.AuditInfo.UpdatedBy);
        result.UpdatedAt.Should().Be(_user.AuditInfo.UpdatedAt);

        _userRepository.Verify(x => x.GetByIdAsync(_user.Id, It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task 異常系_Handle_Userが存在しない場合()
    {
        // Arrange
        _userRepository
            .Setup(x => x.GetByIdAsync(_user.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync((User?)null);

        // Act
        var query = new GetCurrentUserQuery();
        var act = async () => await _handler.Handle(query, CancellationToken.None);

        // Assert
        var ex = await act.Should().ThrowAsync<AuthenticationException>();
        ex.Which.ErrorCode.Should().Be("APPLICATION.USER_NOT_FOUND");
        _userRepository.Verify(x => x.GetByIdAsync(_user.Id, It.IsAny<CancellationToken>()), Times.Once);
    }
}
