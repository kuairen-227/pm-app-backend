using Microsoft.Extensions.DependencyInjection;
using WebApi.Application.Abstractions.AuthService;
using WebApi.Infrastructure.Database;

namespace WebApi.IntegrationTests.Helpers;

public abstract class BaseIntegrationTest
    : IClassFixture<TestWebApplicationFactory>, IAsyncLifetime
{
    protected HttpClient Client { get; }
    protected IServiceScope Scope { get; }
    protected AppDbContext DbContext { get; }
    protected IPasswordHashService PasswordHashService { get; }

    protected BaseIntegrationTest(TestWebApplicationFactory factory)
    {
        Scope = factory.Services.CreateScope();

        DbContext = Scope.ServiceProvider.GetRequiredService<AppDbContext>();
        PasswordHashService = Scope.ServiceProvider
            .GetRequiredService<IPasswordHashService>();

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
