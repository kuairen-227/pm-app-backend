using Microsoft.Extensions.DependencyInjection;
using WebApi.Domain.Aggregates.ProjectAggregate;
using WebApi.Domain.Aggregates.UserAggregate;
using WebApi.Infrastructure.Database;
using WebApi.Tests.Helpers.Builders;

namespace WebApi.IntegrationTests.Helpers;

[Collection("IntegrationTests")]
public abstract class BaseIntegrationTest
    : IClassFixture<TestWebApplicationFactory>, IAsyncLifetime
{
    protected HttpClient Client { get; }
    protected IServiceScope Scope { get; }
    protected AppDbContext DbContext { get; }

    protected BaseIntegrationTest(TestWebApplicationFactory factory)
    {
        factory.Authenticated = true;

        Client = factory.CreateClient();
        Scope = factory.Services.CreateScope();
        DbContext = Scope.ServiceProvider.GetRequiredService<AppDbContext>();
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
        Scope.Dispose();
        return Task.CompletedTask;
    }

    protected async Task<T> SaveAsync<T>(T entity)
        where T : class
    {
        DbContext.Add(entity);
        await DbContext.SaveChangesAsync();
        return entity;
    }

    protected async Task<List<T>> SaveManyAsync<T>(IEnumerable<T> entities)
        where T : class
    {
        var list = entities.ToList();

        DbContext.AddRange(list);
        await DbContext.SaveChangesAsync();

        return list;
    }

    protected async Task<User> LoginAsync()
    {
        var user = new UserBuilder().Build();
        DbContext.Users.Add(user);
        await DbContext.SaveChangesAsync();

        AuthHandler.UserId = user.Id;

        return user;
    }

    protected async Task<(User user, Project project)> LoginWithProjectAsync()
    {
        var user = new UserBuilder().Build();
        var member = new ProjectMemberBuilder()
            .WithUserId(user.Id)
            .WithRole(ProjectRole.RoleType.Member)
            .Build();
        var project = new ProjectBuilder()
            .WithMembers(member)
            .Build();

        DbContext.Users.Add(user);
        DbContext.Projects.Add(project);
        await DbContext.SaveChangesAsync();

        AuthHandler.UserId = user.Id;

        return (user, project);
    }
}
