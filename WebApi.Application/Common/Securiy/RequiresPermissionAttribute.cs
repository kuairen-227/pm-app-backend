namespace WebApi.Application.Common.Security;

[AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
public class RequiresPermissionAttribute : Attribute
{
    public string PermissionCode { get; }

    public RequiresPermissionAttribute(string code)
    {
        PermissionCode = code;
    }
}
