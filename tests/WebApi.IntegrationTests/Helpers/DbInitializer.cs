using Microsoft.EntityFrameworkCore;
using WebApi.Infrastructure.Database;

namespace WebApi.IntegrationTests.Helpers;

public static class DbInitializer
{
    private static readonly SemaphoreSlim _lock = new(1, 1);
    private static bool _initialized;

    public static async Task EnsureInitializedAsync(AppDbContext db)
    {
        await _lock.WaitAsync();
        try
        {
            if (_initialized) return;

            await db.Database.EnsureDeletedAsync();
            await db.Database.MigrateAsync();

            _initialized = true;
        }
        finally
        {
            _lock.Release();
        }
    }
}
