namespace WebApi.Application.Common;

public abstract class ApplicationException : Exception
{
    public string ErrorCode { get; }

    public ApplicationException(string errorCode, string message) : base(message)
    {
        ErrorCode = errorCode;
    }
}

public sealed class NotFoundException : ApplicationException
{
    public NotFoundException(string errorCode, string message)
        : base(errorCode, message) { }
}
