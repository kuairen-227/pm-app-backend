using FluentAssertions;
using MediatR;
using Moq;
using WebApi.Application.Abstractions;
using WebApi.Application.Commands.Auth.RevokeRefreshToken;
using WebApi.Application.Common;
using WebApi.Application.Tests.Helpers.Common;
using WebApi.Domain.Abstractions.Repositories;
using WebApi.Domain.Aggregates.AuthAggregate;
using WebApi.Tests.Helpers.Builders;

namespace WebApi.Application.Tests.Commands.Auth;

public class RevokeRefreshTokenHandlerTests : BaseCommandHandlerTest
{
    private RevokeRefreshTokenHandler _handler;
    private Mock<IRefreshTokenRepository> _refreshTokenRepository;
    private readonly RefreshTokenBuilder _refreshTokenBuilder;

    public RevokeRefreshTokenHandlerTests()
    {
        _refreshTokenRepository = new Mock<IRefreshTokenRepository>();
        _refreshTokenBuilder = new RefreshTokenBuilder();

        _handler = new RevokeRefreshTokenHandler(
            _refreshTokenRepository.Object,
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

        // Act
        var command = new RevokeRefreshTokenCommand(refreshToken.Token);
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().Be(Unit.Value);
        UnitOfWork.Verify(x => x.SaveChangesAsync(
            It.IsAny<IDomainEventPublisher>(), It.IsAny<CancellationToken>()
        ), Times.Once);
    }

    [Fact]
    public async Task 異常系_Handle_RefreshTokenが存在しない場合()
    {
        // Arrange
        var command = new RevokeRefreshTokenCommand("invalid_token");

        _refreshTokenRepository
            .Setup(x => x.GetByTokenAsync(command.RefreshToken, It.IsAny<CancellationToken>()))
            .ReturnsAsync((RefreshToken?)null);

        // Act
        var act = async () => await _handler.Handle(command, CancellationToken.None);

        // Assert
        var ex = await act.Should().ThrowAsync<AuthenticationException>();
        ex.Which.ErrorCode.Should().Be("APPLICATION.INVALID_REFRESH_TOKEN");
    }
}
