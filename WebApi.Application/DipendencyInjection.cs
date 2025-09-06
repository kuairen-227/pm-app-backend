using System.Reflection;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.Extensions.DependencyInjection;

namespace WebApi.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        // MediatR のハンドラをアセンブリから自動登録
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(DependencyInjection).Assembly));

        // services.AddValidatorsFromAssemblyContaining<>;

        // Fluent Validation の自動検証パイプラインを登録
        services.AddFluentValidationAutoValidation();

        return services;
    }
}
