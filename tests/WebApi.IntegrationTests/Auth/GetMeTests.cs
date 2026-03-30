using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using Microsoft.AspNetCore.Identity.Data;
using WebApi.Application.Queries.Users.Dtos;
using WebApi.IntegrationTests.Helpers;

namespace WebApi.IntegrationTests.Auth;

public sealed class GetMeTests : BaseAuthIntegrationTest
{
    private const string BaseUrl = "/api/v1/auth/me";
    private const string LoginUrl = "/api/v1/auth/login";

    public GetMeTests(TestWebApplicationFactory factory)
        : base(factory)
    {
    }

    [Fact]
    public async Task 正常系_ログインユーザーの取得_200()
    {
        // Arrange
        var user = new UserTestDataBuilder(PasswordHashService)
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
        var response = await Client.GetAsync(BaseUrl);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var body = await response.Content.ReadFromJsonAsync<UserDto>();
        body.Should().NotBeNull();
        body.Id.Should().Be(user.User.Id);
        body.Name.Should().Be(user.User.Name);
        body.Email.Should().Be(user.User.Email.ToString());
        body.Role.Should().Be(user.User.Role.ToString());
    }

    [Fact]
    public async Task 異常系_ログインユーザーの取得_401_未認証()
    {
        // Arrange & Act
        var response = await Client.GetAsync(BaseUrl);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }
}
