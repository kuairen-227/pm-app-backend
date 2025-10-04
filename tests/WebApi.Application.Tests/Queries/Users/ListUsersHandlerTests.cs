using FluentAssertions;
using Moq;
using WebApi.Application.Queries.Users.Dtos;
using WebApi.Application.Queries.Users.ListUsers;
using WebApi.Application.Tests.Helpers.Common;
using WebApi.Domain.Abstractions.Repositories;
using WebApi.Domain.Aggregates.UserAggregate;
using WebApi.Tests.Helpers.Builders;

namespace WebApi.Application.Tests.Queries.Users;

public class ListUsersHandlerTests : BaseQueryHandlerTest
{
    private readonly ListUsersHandler _handler;
    private readonly Mock<IUserRepository> _userRepository;
    private readonly UserBuilder _userBuilder;

    public ListUsersHandlerTests()
    {
        _userRepository = new Mock<IUserRepository>();
        _userBuilder = new UserBuilder();

        Mapper.Setup(m => m.Map<IEnumerable<UserDto>>(It.IsAny<IEnumerable<User>>()))
            .Returns<IEnumerable<User>>(users =>
                users.Select(u => new UserDto
                {
                    Id = u.Id,
                    Name = u.Name,
                    Email = u.Email.Value,
                    Role = u.Role.Value.ToString(),
                    CreatedBy = u.CreatedBy,
                    CreatedAt = u.CreatedAt,
                    UpdatedBy = u.UpdatedBy,
                    UpdatedAt = u.UpdatedAt,
                }));

        _handler = new ListUsersHandler(
            _userRepository.Object,
            Mapper.Object
        );
    }

    [Fact]
    public async Task 正常系_Handle()
    {
        // Arrange
        var users = new List<User>
        {
            _userBuilder.WithName("ユーザー1").WithEmail("user1@example.com").WithRole(SystemRole.RoleType.Admin).Build(),
            _userBuilder.WithName("ユーザー2").WithEmail("user2@example.com").WithRole(SystemRole.RoleType.User).Build()
        };

        _userRepository
            .Setup(x => x.GetAllAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(users);

        // Act
        var query = new ListUsersQuery();
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().HaveCount(2);

        for (int i = 0; i < users.Count; i++)
        {
            result.ElementAt(i).Id.Should().Be(users[i].Id);
            result.ElementAt(i).Name.Should().Be(users[i].Name);
            result.ElementAt(i).Email.Should().Be(users[i].Email.Value);
            result.ElementAt(i).Role.Should().Be(users[i].Role.Value.ToString());
            result.ElementAt(i).CreatedBy.Should().Be(users[i].CreatedBy);
            result.ElementAt(i).CreatedAt.Should().Be(users[i].CreatedAt);
            result.ElementAt(i).UpdatedBy.Should().Be(users[i].UpdatedBy);
            result.ElementAt(i).UpdatedAt.Should().Be(users[i].UpdatedAt);
        }

        _userRepository.Verify(x => x.GetAllAsync(It.IsAny<CancellationToken>()), Times.Once);
    }
}
