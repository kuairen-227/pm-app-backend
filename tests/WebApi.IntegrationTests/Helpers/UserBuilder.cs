using WebApi.Application.Abstractions.AuthService;
using WebApi.Domain.Aggregates.ProjectAggregate;
using WebApi.Domain.Aggregates.UserAggregate;
using WebApi.Infrastructure.Database;
using WebApi.Tests.Helpers.Fixtures;

namespace WebApi.IntegrationTests.Helpers;

public class UserBuilder
{
    private readonly IPasswordHashService _passwordHashService;

    private string _name = "Test User";
    private string _email = "test@example.com";
    private string _password = "test-password";
    private SystemRole.RoleType _systemRole = SystemRole.RoleType.User;
    private Guid? _projectId;
    private ProjectRole.RoleType _projectRole = ProjectRole.RoleType.Member;

    public UserBuilder(IPasswordHashService passwordHashService)
    {
        _passwordHashService = passwordHashService;
    }

    public UserBuilder WithName(string name) { _name = name; return this; }
    public UserBuilder WithEmail(string email) { _email = email; return this; }
    public UserBuilder WithPassword(string password) { _password = password; return this; }
    public UserBuilder WithSystemRole(SystemRole.RoleType role) { _systemRole = role; return this; }
    public UserBuilder InProject(Guid projectId, ProjectRole.RoleType projectRole)
    {
        _projectId = projectId;
        _projectRole = projectRole;
        return this;
    }

    public UserWithPassword Build(AppDbContext db)
    {
        var clock = new FakeDateTimeProvider();
        var user = new User(
            _name,
            _email,
            _passwordHashService.Hash(_password),
            _systemRole,
            Guid.Empty,
            clock
        );

        if (_projectId.HasValue)
        {
            // db.ProjectUsers.Add(new ProjectUser(_projectId.Value, user.Id, _projectRole));
        }

        db.Users.Add(user);
        return new UserWithPassword
        {
            User = user,
            Password = _password
        };
    }
}

public sealed class UserWithPassword
{
    public required User User { get; init; }
    public required string Password { get; init; }
}
