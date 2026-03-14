using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using WebApi.Infrastructure.Database;
using WebApi.Infrastructure.Services.AuthService;
using WebApi.IntegrationTests.Helpers;

namespace WebApi.IntegrationTests;

public class TestWebApplicationFactory : WebApplicationFactory<Program>
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureServices(services =>
        {
            // DbContext
            services.RemoveAll<DbContextOptions<AppDbContext>>();

            services.AddDbContext<AppDbContext>(options =>
            {
                options.UseNpgsql(
                    "Host=localhost;Database=test;Username=postgres;Password=postgres");
            });

            // Authentication
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = AuthHandler.TestScheme;
                options.DefaultChallengeScheme = AuthHandler.TestScheme;
            })
            .AddScheme<AuthenticationSchemeOptions, AuthHandler>(
                AuthHandler.TestScheme, _ => { });

            // JWT Settings
            services.Configure<JwtSettings>(options =>
            {
                options.SecretKey = "INTEGRATION_TEST_SECRET_KEY_1234567890";
                options.Issuer = "TestIssuer";
                options.Audience = "TestAudience";
                options.AccessTokenExpirationMinutes = 60;
                options.RefreshTokenExpirationDays = 7;
            });
        });
    }
}
