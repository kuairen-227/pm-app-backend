using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using WebApi.Api.Dtos;
using WebApi.Api.Dtos.Auth;
using WebApi.IntegrationTests.Helpers;

namespace WebApi.IntegrationTests.Auth;

public sealed class RefreshTests : BaseIntegrationTest
{
    private readonly static string BaseUrl = "/api/v1/auth/refresh";
    private readonly static string LoginUrl = "/api/v1/auth/login";

    public RefreshTests(TestWebApplicationFactory factory)
        : base(factory)
    {
    }

    [Fact]
    public async Task 正常系_リフレッシュ_200()
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
            LoginUrl,
            loginRequest
        );
        var cookie = loginResponse.Headers.GetValues("Set-Cookie")
            .First(c => c.StartsWith("refresh_token"));
        Client.DefaultRequestHeaders.Add("Cookie", cookie);

        // Act
        var response = await Client.PostAsync(BaseUrl, null);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var body = await response.Content.ReadFromJsonAsync<RefreshResponse>();
        body.Should().NotBeNull();
        body.UserId.Should().Be(user.User.Id);

        response.Headers.GetValues("Set-Cookie")
            .Should().Contain(c => c.StartsWith("access_token"));
    }

    [Fact]
    public async Task 異常系_リフレッシュ_401()
    {
        // Arrange & Act
        Client.DefaultRequestHeaders.Add("Cookie", "refresh_token=invalid_token");
        var response = await Client.PostAsync(BaseUrl, null);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);

        var error = await response.Content.ReadFromJsonAsync<ErrorResponse>();
        error.Should().NotBeNull();
        error.Error.Code.Should().Be("APPLICATION.INVALID_REFRESH_TOKEN");
    }
}
