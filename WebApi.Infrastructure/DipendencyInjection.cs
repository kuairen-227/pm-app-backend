using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using WebApi.Application.Common;
using WebApi.Domain.Abstractions;
using WebApi.Infrastructure.Common;
using WebApi.Infrastructure.Contexts;
using WebApi.Infrastructure.Database;

namespace WebApi.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, string connectionString)
    {
        // DbContext 登録
        services.AddDbContext<AppDbContext>(options => options.UseNpgsql(connectionString));

        // UserContext 登録
        services.AddScoped<IUserContext, UserContext>();

        // DateTimeProvider 登録
        services.AddSingleton<IDateTimeProvider, DateTimeProvider>();

        return services;
    }
}
