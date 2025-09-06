using FluentAssertions;
using WebApi.Domain.Aggregates.UserAggregate;
using WebApi.Domain.Common;

namespace WebApi.Domain.Tests.Aggregates.UserAggregate;

public class EmailTests
{
    [Fact]
    public void 正常系_インスタンス生成()
    {
        // Act
        var result = Email.Create("test@example.com");

        // Assert
        result.Value.Should().Be("test@example.com");
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    public void 異常系_インスタンス生成_Emailが空の場合(string? input)
    {
        // Act
        Action act = () => Email.Create(input!);

        // Assert
        var ex = act.Should().Throw<DomainException>();
        ex.Which.ErrorCode.Should().Be("EMAIL_REQUIRED");
    }

    [Theory]
    [InlineData("plainAddress")]
    [InlineData("missing-at.com")]
    [InlineData("test@")]
    [InlineData("@example.com")]
    [InlineData("test@@example.com")]
    public void 異常系_インスタンス生成_Emailが不正な場合(string input)
    {
        // Act
        var act = () => Email.Create(input);

        // Assert
        var ex = act.Should().Throw<DomainException>();
        ex.Which.ErrorCode.Should().Be("EMAIL_INVALID");
    }

    [Fact]
    public void 正常系_値が同じ場合()
    {
        // Arrange & Act
        var result1 = Email.Create("test@example.com");
        var result2 = Email.Create("test@example.com");

        // Act
        result1.Should().Be(result2);
        result1.GetHashCode().Should().Be(result2.GetHashCode());
        result1.Equals(result2).Should().BeTrue();
    }

    [Fact]
    public void 正常系_値が異なる場合()
    {
        // Arrange
        var result1 = Email.Create("test@example.com");
        var result2 = Email.Create("test2@example.com");

        // Assert
        result1.Should().NotBe(result2);
        result1.GetHashCode().Should().NotBe(result2.GetHashCode());
        result1.Equals(result2).Should().BeFalse();
    }
}
