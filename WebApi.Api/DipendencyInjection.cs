using Asp.Versioning;

namespace WebApi.Api;

public static class DependencyInjection
{
    public static IServiceCollection AddApi(this IServiceCollection services)
    {
        // Controllers
        services.AddControllers();

        // CORS
        services.AddCors(options =>
        {
            options.AddPolicy("AllowFrontEnd", policy =>
            {
                policy.WithOrigins()
                      .AllowAnyHeader()
                      .AllowAnyMethod();
            });
        });

        // HTTP Context Accessor（UserContextに必要）
        services.AddHttpContextAccessor();

        // API Versioning
        services.AddApiVersioning(options =>
        {
            options.ReportApiVersions = true;
            options.DefaultApiVersion = new ApiVersion(1, 0);
        });

        return services;
    }
}
