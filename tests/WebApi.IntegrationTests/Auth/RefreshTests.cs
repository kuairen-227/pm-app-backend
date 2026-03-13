using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using WebApi.Api.Dtos;
using WebApi.Api.Dtos.Auth;
using WebApi.IntegrationTests.Helpers;

namespace WebApi.IntegrationTests.Auth;

public sealed class RefreshTests : BaseIntegrationTest
{
    private readonly static string baseUrl = "/api/v1/auth/refresh";
    private readonly static string loginUrl = "/api/v1/auth/login";

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
            loginUrl,
            loginRequest
        );
        var loginBody = await loginResponse.Content.ReadFromJsonAsync<LoginResponse>();

        // Act
        var request = new RefreshRequest
        {
            RefreshToken = loginBody!.RefreshToken
        };
        var response = await Client.PostAsJsonAsync(
            baseUrl,
            request
        );

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var body = await response.Content.ReadFromJsonAsync<RefreshResponse>();
        body.Should().NotBeNull();
        body.UserId.Should().Be(user.User.Id);
        body.AccessToken.Should().NotBeNullOrWhiteSpace();
        body.RefreshToken.Should().NotBeNullOrWhiteSpace();
    }

    [Fact]
    public async Task 異常系_リフレッシュ_401()
    {
        // Arrange & Act
        var request = new RefreshRequest
        {
            RefreshToken = "invalid-token"
        };
        var response = await Client.PostAsJsonAsync(
            baseUrl,
            request
        );

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);

        var error = await response.Content.ReadFromJsonAsync<ErrorResponse>();
        error.Should().NotBeNull();
        error.Error.Code.Should().Be("APPLICATION.INVALID_REFRESH_TOKEN");
    }
}
