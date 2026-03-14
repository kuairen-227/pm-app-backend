using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using WebApi.Api.Dtos;
using WebApi.Api.Dtos.Auth;
using WebApi.IntegrationTests.Helpers;

namespace WebApi.IntegrationTests.Auth;

public sealed class LogoutTests : BaseIntegrationTest
{
    private const string BaseUrl = "/api/v1/auth/logout";
    private const string LoginUrl = "/api/v1/auth/login";
    private const string RefreshUrl = "/api/v1/auth/refresh";

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
        var loginResponse = await Client.PostAsJsonAsync(LoginUrl, loginRequest);
        loginResponse.EnsureSuccessStatusCode();

        // Act
        var response = await Client.PostAsync(BaseUrl, null);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NoContent);

        var setCookies = response.Headers.GetValues("Set-Cookie");
        setCookies.Should().Contain(c => c.Contains("access_token=;"));
        setCookies.Should().Contain(c => c.Contains("refresh_token=;"));
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
        var loginResponse = await Client.PostAsJsonAsync(LoginUrl, loginRequest);
        loginResponse.EnsureSuccessStatusCode();

        // Act
        await Client.PostAsync(BaseUrl, null);
        var response = await Client.PostAsync(RefreshUrl, null);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }
}
