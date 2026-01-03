namespace WebApi.Api.Common;

/// <summary>
/// ログ設定
/// </summary>
public static class LoggingExtensions
{
    /// <summary>
    /// カスタムログ設定を追加
    /// </summary>
    public static ILoggingBuilder AddAppLogging(
        this ILoggingBuilder logging,
        IWebHostEnvironment env)
    {
        logging.ClearProviders();

        if (env.IsDevelopment())
        {
            logging.AddSimpleConsole(options =>
            {
                options.SingleLine = false;
                options.TimestampFormat = "HH:mm:ss ";
                options.IncludeScopes = true;
            });
        }
        else
        {
            logging.AddJsonConsole();
        }

        return logging;
    }
}
