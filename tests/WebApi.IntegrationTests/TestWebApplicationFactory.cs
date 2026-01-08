using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using WebApi.Infrastructure.Database;
using WebApi.IntegrationTests.Helpers;

namespace WebApi.IntegrationTests;

public class TestWebApplicationFactory : WebApplicationFactory<Program>
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureServices(services =>
        {
            // DbContext
            var descriptor = services.Single(
                d => d.ServiceType == typeof(DbContextOptions<AppDbContext>));
            services.Remove(descriptor);

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
        });
    }
}
