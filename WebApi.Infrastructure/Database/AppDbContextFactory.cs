using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace WebApi.Infrastructure.Database;

/// <summary>
/// マイグレーション作成用 Factory
/// dotnet ef migrations が設計時に DbContext を生成するために使用
/// </summary>
public class AppDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
{
    public AppDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();

        // マイグレーションファイル生成用のための接続文字列を直接指定
        optionsBuilder
            .UseNpgsql("Host=db;Port=5432;Database=db;Username=postgres;Password=postgres")
            .UseSnakeCaseNamingConvention();

        return new AppDbContext(optionsBuilder.Options);
    }
}
