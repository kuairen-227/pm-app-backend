using System.Diagnostics;

namespace WebApi.Api.Middlewares;

/// <summary>
/// リクエスト毎のロギング用ミドルウェア
/// </summary>
public sealed class HttpRequestLoggingMiddleware
{
    private readonly ILogger<HttpRequestLoggingMiddleware> _logger;
    private readonly RequestDelegate _next;

    /// <summary>
    /// コンストラクタ
    /// </summary>
    public HttpRequestLoggingMiddleware(
        RequestDelegate next,
        ILogger<HttpRequestLoggingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    /// <summary>
    /// ミドルウェア呼び出し
    /// </summary>
    public async Task InvokeAsync(HttpContext context)
    {
        using (_logger.BeginScope(new Dictionary<string, object>
        {
            ["LogSource"] = "App",
            ["Layer"] = "API",
            ["CorrelationId"] = context.TraceIdentifier
        }))
        {
            var stopwatch = Stopwatch.StartNew();

            _logger.LogInformation(
                "HTTP {Method} {Path} started",
                context.Request.Method,
                context.Request.Path
            );

            await _next(context);

            stopwatch.Stop();

            _logger.LogInformation(
                "HTTP {Method} {Path} finished with {StatusCode} in {ElapsedMilliseconds}ms",
                context.Request.Method,
                context.Request.Path,
                context.Response.StatusCode,
                stopwatch.ElapsedMilliseconds
            );
        }
    }
}
