using WebApi.Domain.Aggregates.UserAggregate;
using WebApi.Domain.Helpers.Common;

namespace WebApi.Domain.Tests.Helpers;

public class UserBuilder : BaseBuilder<UserBuilder, User>
{
    private string _name = "デフォルトユーザー";
    private string _email = "default@example.com";
    private SystemRole.RoleType _role = SystemRole.RoleType.User;

    public UserBuilder WithName(string name)
    {
        _name = name;
        return this;
    }

    public UserBuilder WithEmail(string email)
    {
        _email = email;
        return this;
    }

    public UserBuilder WithRole(SystemRole.RoleType role)
    {
        _role = role;
        return this;
    }

    public override User Build()
    {
        return new User(
            _name,
            Email.Create(_email),
            SystemRole.Create(_role),
            _createdBy,
            _clock
        );
    }
}
