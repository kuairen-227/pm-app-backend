namespace WebApi.Api.Middlewares;

/// <summary>
/// リクエストに CorrelationId を付与する拡張メソッド
/// </summary>
public sealed class CorrelationIdMiddleware
{
    private const string HeaderName = "X-Correlation-Id";

    private readonly RequestDelegate _next;

    /// <summary>
    /// コンストラクタ
    /// </summary>
    public CorrelationIdMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    /// <summary>
    /// ミドルウェア呼び出し
    /// </summary>
    public async Task InvokeAsync(HttpContext context)
    {
        var correlationId =
            context.Request.Headers[HeaderName].FirstOrDefault()
            ?? Guid.NewGuid().ToString();

        context.Response.Headers[HeaderName] = correlationId;

        using (_ = context.RequestServices
            .GetRequiredService<ILoggerFactory>()
            .CreateLogger("Correlation")
            .BeginScope(new Dictionary<string, object>
            {
                ["CorrelationId"] = correlationId
            }))
        {
            await _next(context);
        }
    }
}
