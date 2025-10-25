using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using WebApi.Application.Abstractions;
using WebApi.Domain.Abstractions;
using WebApi.Infrastructure.Common;
using WebApi.Infrastructure.Contexts;
using WebApi.Infrastructure.Database;

namespace WebApi.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, string connectionString)
    {
        // DbContext
        services.AddDbContext<AppDbContext>(options => options.UseNpgsql(connectionString));

        // UserContext
        services.AddScoped<IUserContext, UserContext>();

        // DateTimeProvider
        services.AddSingleton<IDateTimeProvider, DateTimeProvider>();

        return services;
    }
}
