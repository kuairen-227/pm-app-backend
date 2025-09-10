namespace WebApi.Application.Common;

public abstract class ApplicationException : Exception
{
    public string ErrorCode { get; }

    public ApplicationException(string errorCode, string message) : base(message)
    {
        ErrorCode = errorCode;
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
    public NotFoundException(string errorCode, string message)
        : base(errorCode, message) { }
}
