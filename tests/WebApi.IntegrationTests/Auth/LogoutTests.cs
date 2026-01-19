using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using WebApi.Api.Dtos;
using WebApi.Api.Dtos.Auth;
using WebApi.IntegrationTests.Helpers;

namespace WebApi.IntegrationTests.Auth;

public sealed class LogoutTests : BaseIntegrationTest
{
    private readonly static string baseUrl = "/api/v1/auth/logout";
    private readonly static string loginUrl = "/api/v1/auth/login";
    private readonly static string refreshUrl = "/api/v1/auth/refresh";

    public LogoutTests(TestWebApplicationFactory factory)
        : base(factory)
    {
    }

    [Fact]
    public async Task 正常系_ログアウト_204()
    {
        // Arrange
        var user = new UserBuilder(PasswordHashService)
            .Build(DbContext);
        await DbContext.SaveChangesAsync();

        var loginRequest = new LoginRequest
        {
            Email = user.User.Email.Value,
            Password = user.Password
        };
        var loginResponse = await Client.PostAsJsonAsync(
            loginUrl,
            loginRequest
        );
        var loginBody = await loginResponse.Content.ReadFromJsonAsync<LoginResponse>();

        // Act
        var request = new LogoutRequest
        {
            RefreshToken = loginBody!.RefreshToken
        };
        var response = await Client.PostAsJsonAsync(
            baseUrl,
            request
        );

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NoContent);
    }

    [Fact]
    public async Task 異常系_ログアウト_401_ログアウト後はリフレッシュ不可()
    {
        // Arrange
        var user = new UserBuilder(PasswordHashService)
            .Build(DbContext);
        await DbContext.SaveChangesAsync();

        var loginRequest = new LoginRequest
        {
            Email = user.User.Email.Value,
            Password = user.Password
        };
        var loginResponse = await Client.PostAsJsonAsync(
            loginUrl,
            loginRequest
        );
        var loginBody = await loginResponse.Content.ReadFromJsonAsync<LoginResponse>();

        var logoutRequest = new LogoutRequest
        {
            RefreshToken = loginBody!.RefreshToken
        };
        await Client.PostAsJsonAsync(
            baseUrl,
            logoutRequest
        );

        // Act
        var request = new RefreshRequest
        {
            RefreshToken = loginBody!.RefreshToken
        };
        var response = await Client.PostAsJsonAsync(
            refreshUrl,
            request
        );

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);

        var error = await response.Content.ReadFromJsonAsync<ErrorResponse>();
        error.Should().NotBeNull();
        error.Error.Code.Should().Be("APPLICATION.INVALID_REFRESH_TOKEN");
    }
}
