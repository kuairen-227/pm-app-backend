using Asp.Versioning;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using WebApi.Api.Common;

namespace WebApi.Api;

/// <summary>
/// API層のDI
/// </summary>
public static class DependencyInjection
{
    /// <summary>
    /// API層のDIを追加
    /// </summary>
    public static IServiceCollection AddApi(
        this IServiceCollection services, IConfigurationSection jwtSection)
    {
        // Controllers
        services.AddControllers()
            .AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.Converters
                    .Add(new PatchFieldJsonConverterFactory());
            });

        // Swagger
        services.AddOpenApi();
        services.AddOpenApiDocument(options =>
        {
            options.Title = "pm-app API";
        });

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

        // Authentication
        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new()
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = jwtSection["Issuer"],
                    ValidAudience = jwtSection["Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(
                        System.Text.Encoding.UTF8.GetBytes(
                            jwtSection["SecretKey"] ?? throw new InvalidOperationException("Jwt::SecretKey は必須です")))
                };

                options.Events = new JwtBearerEvents
                {
                    OnMessageReceived = context =>
                    {
                        if (context.Request.Cookies.TryGetValue("access_token", out var token))
                        {
                            context.Token = token;
                        }

                        return Task.CompletedTask;
                    }
                };
            });

        return services;
    }
}
