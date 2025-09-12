using WebApi.Application.Helpers.Common;
using WebApi.Domain.Aggregates.UserAggregate;

namespace WebApi.Application.Tests.Helpers;

public class UserBuilder : BaseBuilder<UserBuilder, User>
{
    private string _name = "デフォルトユーザー";
    private string _email = "default@example.com";
    private Role.RoleType _role = Role.RoleType.Member;

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

    public UserBuilder WithRole(Role.RoleType role)
    {
        _role = role;
        return this;
    }

    public override User Build()
    {
        return new User(
            _name,
            Email.Create(_email),
            Role.Create(_role),
            _createdBy,
            _clock
        );
    }
}
