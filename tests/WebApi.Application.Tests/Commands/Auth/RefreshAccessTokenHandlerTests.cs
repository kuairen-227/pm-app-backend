using FluentAssertions;
using Moq;
using WebApi.Application.Abstractions.AuthService;
using WebApi.Application.Commands.Auth.RefreshAccessToken;
using WebApi.Application.Common;
using WebApi.Application.Tests.Helpers.Common;
using WebApi.Domain.Abstractions.Repositories;
using WebApi.Tests.Helpers.Builders;

namespace WebApi.Application.Tests.Commands.Auth;

public class RefreshAccessTokenHandlerTests : BaseCommandHandlerTest
{
    private readonly RefreshAccessTokenHandler _handler;
    private readonly Mock<IRefreshTokenRepository> _refreshTokenRepository;
    private readonly Mock<IJwtService> _jwtService;
    private readonly RefreshTokenBuilder _refreshTokenBuilder;

    public RefreshAccessTokenHandlerTests()
    {
        _refreshTokenRepository = new Mock<IRefreshTokenRepository>();
        _jwtService = new Mock<IJwtService>();
        _refreshTokenBuilder = new RefreshTokenBuilder();

        _handler = new RefreshAccessTokenHandler(
            _refreshTokenRepository.Object,
            _jwtService.Object,
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
        var refreshToken = _refreshTokenBuilder.Build();

        _refreshTokenRepository
            .Setup(x => x.GetByTokenAsync(refreshToken.Token, It.IsAny<CancellationToken>()))
            .ReturnsAsync(refreshToken);

        _jwtService
            .Setup(x => x.GenerateAccessToken(refreshToken.UserId))
            .Returns("new_access_token");

        // Act
        var command = new RefreshAccessTokenCommand(refreshToken.Token);
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.UserId.Should().Be(refreshToken.UserId);
        result.AccessToken.Should().Be("new_access_token");
        result.RefreshToken.Should().Be(refreshToken.Token);
    }

    [Fact]
    public async Task 異常系_Handle_RefreshTokenが存在しない場合()
    {
        // Arrange
        var refreshToken = _refreshTokenBuilder.Build();

        _refreshTokenRepository
            .Setup(x => x.GetByTokenAsync(refreshToken.Token, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Domain.Aggregates.AuthAggregate.RefreshToken?)null);

        // Act
        var command = new RefreshAccessTokenCommand(refreshToken.Token);
        var act = async () => await _handler.Handle(command, CancellationToken.None);

        // Assert
        var ex = await act.Should().ThrowAsync<AuthenticationException>();
        ex.Which.ErrorCode.Should().Be("APPLICATION.INVALID_REFRESH_TOKEN");
    }
}
