using Microsoft.Extensions.DependencyInjection;
using WebApi.Domain.Services.NotificationFactories;

namespace WebApi.Domain;

public static class DependencyInjection
{
    public static IServiceCollection AddDomain(this IServiceCollection services)
    {
        // ドメインサービス 登録
        services.AddSingleton<ProjectNotificationFactory>();

        return services;
    }
}
