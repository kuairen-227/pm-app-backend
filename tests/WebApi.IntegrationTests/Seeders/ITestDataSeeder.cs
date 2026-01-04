using WebApi.Infrastructure.Database;

namespace WebApi.IntegrationTests.Seeders;

public interface ITestDataSeeder
{
    Task SeedAsync(AppDbContext dbContext);
}
