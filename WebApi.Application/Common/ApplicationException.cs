namespace WebApi.Application.Common;

public class ApplicationException : Exception
{
    public string ErrorCode { get; }

    public ApplicationException(string errorCode, string message) : base(message)
    {
        ErrorCode = $"APPLICATION.{errorCode}";
    }
}

public sealed class AuthenticationException : ApplicationException
{
    public AuthenticationException(string errorCode, string message)
        : base(errorCode, message) { }
}

public sealed class AuthorizationException : ApplicationException
{
    public AuthorizationException(string errorCode, string message)
        : base(errorCode, message) { }
}

public sealed class NotFoundException : ApplicationException
{
    public NotFoundException(string entityName, Guid key)
        : base(
            $"{entityName.ToUpperInvariant()}_NOT_FOUND",
            $"{entityName}({key}) が見つかりません"
        )
    {
    }
}
