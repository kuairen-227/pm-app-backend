using Asp.Versioning;

namespace WebApi.Api;

/// <summary>
/// API層のDI
/// </summary>
public static class DependencyInjection
{
    /// <summary>
    /// API層のDIを追加
    /// </summary>
    public static IServiceCollection AddApi(this IServiceCollection services)
    {
        // Controllers
        services.AddControllers();

        // Swagger
        services.AddOpenApi();
        services.AddOpenApiDocument();

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
