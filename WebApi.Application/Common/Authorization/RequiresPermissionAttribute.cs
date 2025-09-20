namespace WebApi.Application.Common.Authorization;

[AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
public class RequiresPermissionAttribute : Attribute
{
    public string PermissionCode { get; }

    public RequiresPermissionAttribute(string permissionCode)
    {
        PermissionCode = permissionCode;
    }
}
