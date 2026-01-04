namespace WebApi.IntegrationTests.Helpers;

public abstract class BaseIntegrationTest
    : IClassFixture<TestWebApplicationFactory>, IAsyncLifetime
{
    protected HttpClient Client { get; }
    protected IServiceScope Scope { get; }
    protected AppDbContext DbContext { get; }

    protected BaseIntegrationTest(TestWebApplicationFactory factory)
    {
        Client = factory.CreateClient();

        Scope = factory.Services.CreateScope();
        DbContext = Scope.ServiceProvider.GetRequiredService<AppDbContext>();
    }

    public async Task InitializeAsync()
    {
        await DbCleaner.CleanAsync(DbContext);
    }

    public Task DisposeAsync()
    {
        Scope.Dispose();
        Client.Dispose();
        return Task.CompletedTask;
    }
}
