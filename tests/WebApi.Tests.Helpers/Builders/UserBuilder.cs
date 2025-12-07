using WebApi.Domain.Aggregates.UserAggregate;
using WebApi.Tests.Helpers.Builders.Common;

namespace WebApi.Tests.Helpers.Builders;

public class UserBuilder : BaseBuilder<UserBuilder, User>
{
    private string _name = "デフォルトユーザー";
    private string _email = "default@example.com";
    private string _password = "defaultPassword";
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

    public UserBuilder WithPassword(string password)
    {
        _password = password;
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
            _email,
            _password,
            _role,
            _createdBy,
            _clock
        );
    }
}
