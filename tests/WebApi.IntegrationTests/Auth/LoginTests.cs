using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using WebApi.Api.Dtos;
using WebApi.Api.Dtos.Auth;
using WebApi.IntegrationTests.Helpers;

namespace WebApi.IntegrationTests.Auth;

public sealed class LoginTests : BaseIntegrationTest
{
    private const string BaseUrl = "/api/v1/auth/login";

    public LoginTests(TestWebApplicationFactory factory)
        : base(factory)
    {
    }

    [Fact]
    public async Task 正常系_ログイン_200()
    {
        // Arrange
        var user = new UserBuilder(PasswordHashService)
            .Build(DbContext);
        await DbContext.SaveChangesAsync();

        // Act
        var request = new LoginRequest
        {
            Email = user.User.Email.Value,
            Password = user.Password
        };
        var response = await Client.PostAsJsonAsync(
            BaseUrl,
            request
        );

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var body = await response.Content.ReadFromJsonAsync<LoginResponse>();
        body.Should().NotBeNull();
        body.UserId.Should().Be(user.User.Id);

        var cookies = response.Headers.GetValues("Set-Cookie");
        cookies.Should().Contain(c => c.StartsWith("access_token="));
        cookies.Should().Contain(c => c.StartsWith("refresh_token="));
        cookies.Should().Contain(c => c.Contains("httponly"));
    }

    [Fact]
    public async Task 異常系_ログイン_401()
    {
        // Arrange
        var user = new UserBuilder(PasswordHashService)
            .Build(DbContext);
        await DbContext.SaveChangesAsync();

        // Act
        var request = new LoginRequest
        {
            Email = user.User.Email.Value,
            Password = "invalid" + user.Password
        };
        var response = await Client.PostAsJsonAsync(
            BaseUrl,
            request
        );

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);

        var error = await response.Content.ReadFromJsonAsync<ErrorResponse>();
        error.Should().NotBeNull();
        error.Error.Code.Should().Be("APPLICATION.INVALID_CREDENTIAL");
    }
}
