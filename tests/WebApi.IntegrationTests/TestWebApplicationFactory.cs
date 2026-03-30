using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using WebApi.Infrastructure.Database;
using WebApi.IntegrationTests.Helpers;

namespace WebApi.IntegrationTests;

public class TestWebApplicationFactory : WebApplicationFactory<Program>
{
    public bool Authenticated { get; set; }

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseEnvironment("Testing");

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
            if (Authenticated)
            {
                services.AddAuthentication(options =>
                {
                    options.DefaultAuthenticateScheme = AuthHandler.TestScheme;
                    options.DefaultChallengeScheme = AuthHandler.TestScheme;
                })
                .AddScheme<AuthenticationSchemeOptions, AuthHandler>(
                    AuthHandler.TestScheme, _ => { });
            }
        }
        );
    }
}
