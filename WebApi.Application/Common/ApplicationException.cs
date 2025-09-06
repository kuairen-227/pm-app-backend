namespace WebApi.Application.Common;

public abstract class ApplicationException : Exception
{
    public string Code { get; }

    public ApplicationException(string code, string message) : base(message)
    {
        Code = code;
    }
}

public sealed class NotFoundException : ApplicationException
{
    public NotFoundException(string code, string message)
        : base(code, message) { }
}
