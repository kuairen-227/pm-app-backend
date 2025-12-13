using FluentAssertions;
using Moq;
using WebApi.Application.Abstractions;
using WebApi.Application.Commands.Auth.GenerateRefreshToken;
using WebApi.Application.Tests.Helpers.Common;
using WebApi.Domain.Abstractions.Repositories;
using WebApi.Domain.Aggregates.AuthAggregate;
using WebApi.Tests.Helpers.Builders;

namespace WebApi.Application.Tests.Commands.Auth;

public class GenerateRefreshTokenHandlerTests : BaseCommandHandlerTest
{
    private readonly GenerateRefreshTokenHandler _handler;
    private readonly Mock<IRefreshTokenRepository> _refreshTokenRepository;
    private readonly Mock<ITokenService> _tokenService;
    private readonly RefreshTokenBuilder refreshTokenBuilder;

    public GenerateRefreshTokenHandlerTests()
    {
        _refreshTokenRepository = new Mock<IRefreshTokenRepository>();
        _tokenService = new Mock<ITokenService>();
        refreshTokenBuilder = new RefreshTokenBuilder();

        _handler = new GenerateRefreshTokenHandler(
            _refreshTokenRepository.Object,
            _tokenService.Object,
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
        var refreshToken = refreshTokenBuilder.Build();

        _tokenService
            .Setup(x => x.GenerateSecureToken(It.IsAny<int>()))
            .Returns(refreshToken.Token);

        RefreshToken? capturedRefreshToken = null;
        _refreshTokenRepository
            .Setup(x => x.AddAsync(It.IsAny<RefreshToken>(), It.IsAny<CancellationToken>()))
            .Callback<RefreshToken, CancellationToken>((rt, _) => capturedRefreshToken = rt)
            .Returns(Task.CompletedTask);

        // Act
        var command = new GenerateRefreshTokenCommand();
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.RefreshToken.Should().Be(refreshToken.Token);

        capturedRefreshToken.Should().NotBeNull();
        capturedRefreshToken.UserId.Should().Be(UserContext.Object.Id);
        capturedRefreshToken.Token.Should().NotBeNullOrEmpty(refreshToken.Token);
        capturedRefreshToken.ExpiresAt.Should().Be(Clock.Now.AddDays(7));
        capturedRefreshToken.AuditInfo.CreatedBy.Should().Be(UserContext.Object.Id);
        capturedRefreshToken.AuditInfo.CreatedAt.Should().Be(Clock.Now);
    }
}
