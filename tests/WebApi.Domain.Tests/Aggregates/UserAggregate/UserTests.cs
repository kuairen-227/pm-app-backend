using FluentAssertions;
using WebApi.Domain.Aggregates.UserAggregate;
using WebApi.Domain.Common;
using WebApi.Domain.Tests.Helpers;
using WebApi.Tests.Helpers.Builders;

namespace WebApi.Domain.Tests.Aggregates.UserAggregate;

public class UserTests : BaseDomainTest
{
    private readonly UserBuilder _userBuilder;

    public UserTests()
    {
        _userBuilder = new UserBuilder();
    }

    [Fact]
    public void 正常系_インスタンス生成()
    {
        // Arrange & Act
        var result = _userBuilder.Build();

        // Assert
        result.Should().NotBeNull();
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    public void 異常系_インスタンス生成_Nameが空の場合(string? name)
    {
        // Arrange & Act
        var act = () => _userBuilder.WithName(name!).Build();

        // Assert
        var ex = act.Should().Throw<DomainException>();
        ex.Which.ErrorCode.Should().Be("DOMAIN.USER_NAME_REQUIRED");
    }

    [Fact]
    public void 正常系_ChangeName()
    {
        // Arrange
        var user = _userBuilder.Build();

        // Act
        var newName = "New Name";
        user.ChangeName(newName, Clock);

        // Assert
        user.Name.Should().Be(newName);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    public void 異常系_ChangeName_Nameが空の場合(string? name)
    {
        // Arrange
        var user = _userBuilder.Build();

        // Act
        var act = () => user.ChangeName(name!, Clock);

        // Assert
        var ex = act.Should().Throw<DomainException>();
        ex.Which.ErrorCode.Should().Be("DOMAIN.USER_NAME_REQUIRED");
    }

    [Fact]
    public void 正常系_ChangeEmail()
    {
        // Arrange
        var user = _userBuilder.Build();

        // Act
        var newEmail = "new@example.com";
        user.ChangeEmail(newEmail, Clock);

        // Assert
        user.Email.Value.Should().Be(newEmail);
    }

    [Fact]
    public void 正常系_ChangePassword()
    {
        // Arrange
        var user = _userBuilder.Build();

        // Act
        var newPasswordHash = "newPasswordHash";
        user.ChangePassword(newPasswordHash, Clock);

        // Assert
        user.PasswordHash.Should().Be(newPasswordHash);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    public void 異常系_ChangePassword_PasswordHashが空の場合(string? password)
    {
        // Arrange
        var user = _userBuilder.Build();

        // Act
        var act = () => user.ChangePassword(password!, Clock);

        // Assert
        var ex = act.Should().Throw<DomainException>();
        ex.Which.ErrorCode.Should().Be("DOMAIN.USER_PASSWORD_REQUIRED");
    }

    [Fact]
    public void 正常系_ChangeUserRole()
    {
        // Arrange
        var user = _userBuilder.WithRole(SystemRole.RoleType.User).Build();

        // Act
        var newRole = SystemRole.RoleType.Admin;
        user.ChangeUserRole(newRole, Clock);

        // Assert
        user.Role.Value.Should().Be(newRole);
    }
}
