namespace WebApi.Domain.Common.Security;

public sealed class Permission
{
    public string Code { get; }

    private Permission(string code)
    {
        Code = code;
    }

    public static Permission Create(string code) => new Permission(code);
    public override string ToString() => Code;
}
