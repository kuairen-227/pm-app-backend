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
        // Migration（初回のみ）
        await TestDbInitializer.EnsureInitializedAsync(DbContext);

        // Seed
        var userSeeder = new UserSeeder();
        var userId = await userSeeder.SeedAsync(DbContext);

        // HttpClient（認証込み）
        TestAuthHandler.UserId = userId;
    }

    public Task DisposeAsync()
    {
        DbContext.Dispose();
        Scope.Dispose();
        Client.Dispose();
        return Task.CompletedTask;
    }
}
