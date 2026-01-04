using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using WebApi.Infrastructure.Database;
using WebApi.IntegrationTests.Seeders;

namespace WebApi.IntegrationTests.Helpers;

public abstract class BaseIntegrationTest
    : IClassFixture<TestWebApplicationFactory>, IAsyncLifetime
{
    protected HttpClient Client { get; }
    protected IServiceScope Scope { get; }
    protected AppDbContext DbContext { get; }

    protected BaseIntegrationTest(TestWebApplicationFactory factory)
    {
        // DbContext
        Scope = factory.Services.CreateScope();
        DbContext = Scope.ServiceProvider.GetRequiredService<AppDbContext>();

        Client = factory.CreateClient();
    }

    public async Task InitializeAsync()
    {
        // Clean
        await DbCleaner.CleanAsync(DbContext);

        // Seed
        var userSeeder = new UserSeeder();
        userSeeder.SeedAsync(DbContext).GetAwaiter().GetResult();

        // HttpClient（認証込み）
        TestAuthHandler.UserId = userSeeder.UserId;
    }

    public Task DisposeAsync()
    {
        DbContext.Dispose();
        Scope.Dispose();
        Client.Dispose();
        return Task.CompletedTask;
    }
}
