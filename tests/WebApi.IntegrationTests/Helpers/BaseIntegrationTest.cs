using Microsoft.Extensions.DependencyInjection;
using WebApi.Infrastructure.Database;

namespace WebApi.IntegrationTests.Helpers;

public abstract class BaseIntegrationTest
    : IClassFixture<TestWebApplicationFactory>, IAsyncLifetime
{
    protected HttpClient Client { get; }
    protected IServiceScope Scope { get; }
    protected AppDbContext DbContext { get; }

    protected BaseIntegrationTest(TestWebApplicationFactory factory)
    {
        Scope = factory.Services.CreateScope();
        DbContext = Scope.ServiceProvider.GetRequiredService<AppDbContext>();
        Client = factory.CreateClient();
    }

    public async Task InitializeAsync()
    {
        // Migration（初回のみ）
        await DbInitializer.EnsureInitializedAsync(DbContext);

        // Clean
        await DbCleaner.CleanAsync(DbContext);
    }

    public Task DisposeAsync()
    {
        DbContext.Dispose();
        Scope.Dispose();
        Client.Dispose();
        return Task.CompletedTask;
    }
}
