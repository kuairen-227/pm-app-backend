namespace WebApi.Api.Middlewares;

/// <summary>
/// カスタムミドルウェアの登録をまとめる拡張メソッド
/// </summary>
public static class ApplicationBuilderExtensions
{
    /// <summary>
    /// ミドルウェアの登録をまとめる拡張メソッド
    /// </summary>
    public static IApplicationBuilder UseCustomMiddleware(this IApplicationBuilder app)
    {
        app.UseMiddleware<ExceptionHandlingMiddleware>();

        return app;
    }
}
