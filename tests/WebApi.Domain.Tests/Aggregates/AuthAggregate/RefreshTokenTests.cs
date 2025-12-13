using FluentAssertions;
using WebApi.Domain.Common;
using WebApi.Domain.Tests.Helpers;
using WebApi.Tests.Helpers.Builders;

namespace WebApi.Domain.Tests.Aggregates.AuthAggregate;

public class RefreshTokenTests : BaseDomainTest
{
    private readonly RefreshTokenBuilder _refreshTokenBuilder;

    public RefreshTokenTests()
    {
        _refreshTokenBuilder = new RefreshTokenBuilder();
    }

    [Fact]
    public void 正常系_インスタンス生成()
    {
        // Arrange & Act
        var result = _refreshTokenBuilder.Build();

        // Assert
        result.Should().NotBeNull();
        result.IsRevoked.Should().BeFalse();
    }

    [Fact]
    public void 異常系_インスタンス生成_RefreshTokenが空()
    {
        // Arrange
        var builder = _refreshTokenBuilder.WithToken(string.Empty);

        // Act
        var act = () => builder.Build();

        // Assert
        var ex = act.Should().Throw<DomainException>();
        ex.Which.ErrorCode.Should().Be("DOMAIN.REFRESH_TOKEN_REQUIRED");
    }

    [Fact]
    public void 異常系_インスタンス生成_ExpiresAtが過去の場合()
    {
        // Arrange
        var builder = _refreshTokenBuilder.WithExpiresAt(Clock.Now.AddSeconds(-1));

        // Act
        var act = () => builder.Build();

        // Assert
        var ex = act.Should().Throw<DomainException>();
        ex.Which.ErrorCode.Should().Be("DOMAIN.REFRESH_TOKEN_INVALID_EXPIRY");
    }

    [Fact]
    public void 正常系_Revoke()
    {
        // Arrange
        var refreshToken = _refreshTokenBuilder.Build();

        // Act
        refreshToken.Revoke(refreshToken.UserId, Clock);

        // Assert
        refreshToken.IsRevoked.Should().BeTrue();
        refreshToken.RevokedAt.Should().Be(Clock.Now);
    }

    [Fact]
    public void 異常系_Revoke_Revoke済の場合()
    {
        // Arrange
        var refreshToken = _refreshTokenBuilder.WithIsRevoked(true).Build();

        // Act
        var act = () => refreshToken.Revoke(refreshToken.UserId, Clock);

        // Assert
        var ex = act.Should().Throw<DomainException>();
        ex.Which.ErrorCode.Should().Be("DOMAIN.REFRESH_TOKEN_ALREADY_REVOKED");
    }
}
