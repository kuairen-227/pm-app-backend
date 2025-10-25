using Microsoft.Extensions.DependencyInjection;
using WebApi.Domain.Services.NotificationFactories;

namespace WebApi.Domain;

public static class DependencyInjection
{
    public static IServiceCollection AddDomain(this IServiceCollection services)
    {
        // Domain Services
        services.AddSingleton<ProjectNotificationFactory>();

        return services;
    }
}
