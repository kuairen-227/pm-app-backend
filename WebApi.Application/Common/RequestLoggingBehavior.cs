using System.Diagnostics;
using MediatR;
using Microsoft.Extensions.Logging;

namespace WebApi.Application.Common;

public sealed class RequestLoggingBehavior<TRequest, TResponse>
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : notnull
{
    private readonly ILogger<RequestLoggingBehavior<TRequest, TResponse>> _logger;

    public RequestLoggingBehavior(
        ILogger<RequestLoggingBehavior<TRequest, TResponse>> logger)
    {
        _logger = logger;
    }

    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        using (_logger.BeginScope(new Dictionary<string, object>
        {
            ["LogSource"] = "App",
            ["Layer"] = "Application",
            ["RequestName"] = typeof(TRequest).Name
        }))
        {
            var requestName = typeof(TRequest).Name;
            var stopwatch = Stopwatch.StartNew();

            _logger.LogInformation(
                "Handling request {RequestName}",
                requestName
            );

            try
            {
                var response = await next();

                stopwatch.Stop();

                _logger.LogInformation(
                    "Handled request {RequestName} in {ElapsedMilliseconds}ms",
                    requestName,
                    stopwatch.ElapsedMilliseconds
                );

                return response;
            }
            catch
            {
                stopwatch.Stop();

                _logger.LogError(
                    "Request {RequestName} failed after {ElapsedMilliseconds}ms",
                    requestName,
                    stopwatch.ElapsedMilliseconds
                );

                throw;
            }
        }
    }
}
