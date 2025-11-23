namespace WebApi.Domain.Common;

public class DomainException : Exception
{
    public string ErrorCode { get; }

    public DomainException(string errorCode, string message, Exception? inner = null) : base(message, inner)
    {
        ErrorCode = $"DOMAIN.{errorCode}";
    }
}
