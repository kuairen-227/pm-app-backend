using FluentAssertions;
using WebApi.Domain.Common;
using WebApi.Tests.Helpers.Builders;

namespace WebApi.Domain.Tests.Aggregates.UserAggregate;

public class UserTests
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
}
