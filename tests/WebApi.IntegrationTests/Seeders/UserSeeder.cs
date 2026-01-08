using Microsoft.EntityFrameworkCore;
using WebApi.Domain.Aggregates.UserAggregate;
using WebApi.Domain.Common;
using WebApi.Infrastructure.Database;
using WebApi.Tests.Helpers.Fixtures;

namespace WebApi.IntegrationTests.Seeders;

public class UserSeeder
{
    public Guid UserId { get; private set; }

    public async Task<Guid> SeedAsync(AppDbContext dbContext)
    {
        var clock = new FakeDateTimeProvider();

        var admin = new User(
            name: "Test Admin",
            email: "test-admin@example.com",
            passwordHash: "hashed-password",
            role: SystemRole.RoleType.Admin,
            createdBy: Guid.Empty,
            clock: clock
        );

        var user = new User(
            name: "Test User",
            email: "test-user@example.com",
            passwordHash: "hashed-password",
            role: SystemRole.RoleType.User,
            createdBy: Guid.Empty,
            clock: clock
        );

        dbContext.Users.AddRange(admin, user);
        await dbContext.SaveChangesAsync();

        return user.Id;
    }
}
