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
        var email = Email.Create("test@example.com");

        // Assert
        email.Value.Should().Be("test@example.com");
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
        var ex = act.Should().Throw<DomainException>().Which;
        ex.ErrorCode.Should().Be("EMAIL_REQUIRED");
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
        var ex = act.Should().Throw<DomainException>().Which;
        ex.ErrorCode.Should().Be("EMAIL_INVALID");
    }

    [Fact]
    public void 正常系_値が同じ場合()
    {
        // Arrange & Act
        var email1 = Email.Create("test@example.com");
        var email2 = Email.Create("test@example.com");

        // Act
        email1.Should().Be(email2);
        email1.GetHashCode().Should().Be(email2.GetHashCode());
        email1.Equals(email2).Should().BeTrue();
    }

    [Fact]
    public void 正常系_値が異なる場合()
    {
        // Arrange
        var email1 = Email.Create("test@example.com");
        var email2 = Email.Create("test2@example.com");

        // Assert
        email1.Should().NotBe(email2);
        email1.GetHashCode().Should().NotBe(email2.GetHashCode());
        email1.Equals(email2).Should().BeFalse();
    }
}
