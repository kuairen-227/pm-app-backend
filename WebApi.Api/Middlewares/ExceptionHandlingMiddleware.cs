using WebApi.Api.Dtos;
using WebApi.Application.Common;
using WebApi.Domain.Common;
using ApplicationException = WebApi.Application.Common.ApplicationException;

namespace WebApi.Api.Middlewares;

/// <summary>
/// エラーハンドリング用のカスタムミドルウェア
/// </summary>
public class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionHandlingMiddleware> _logger;

    /// <summary>
    /// コンストラクタ
    /// </summary>
    public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    /// <summary>
    /// ミドルウェア呼び出し
    /// </summary>
    public async Task Invoke(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            var errorResponse = CreateErrorResponse(context, ex, out int statusCode);

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = statusCode;

            _logger.LogError(ex, "Unhandled exception: {TraceId}", context.TraceIdentifier);

            await context.Response.WriteAsJsonAsync(errorResponse);
        }
    }

    private ErrorResponse CreateErrorResponse(HttpContext context, Exception ex, out int statusCode)
    {
        statusCode = ex switch
        {
            NotFoundException => StatusCodes.Status404NotFound,
            AuthenticationException => StatusCodes.Status401Unauthorized,
            AuthorizationException => StatusCodes.Status403Forbidden,
            DomainException => StatusCodes.Status400BadRequest,
            ApplicationException => StatusCodes.Status400BadRequest,
            _ => StatusCodes.Status500InternalServerError
        };

        string code = ex switch
        {
            ApplicationException ae => ae.ErrorCode,
            DomainException de => de.ErrorCode,
            _ => "INTERNAL_ERROR"
        };

        return new ErrorResponse
        {
            Error = new ErrorDetail
            {
                Code = code,
                Message = ex.Message,
                TraceId = context.TraceIdentifier
            }
        };
    }
}
