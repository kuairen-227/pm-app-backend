using WebApi.Application.Abstractions.AuthService;
using WebApi.Domain.Aggregates.ProjectAggregate;
using WebApi.Domain.Aggregates.UserAggregate;
using WebApi.Infrastructure.Database;
using WebApi.Tests.Helpers.Fixtures;

namespace WebApi.IntegrationTests.Helpers;

public class UserTestDataBuilder
{
    private readonly IPasswordHashService _passwordHashService;

    private string _name = "Test User";
    private string _email = "test@example.com";
    private string _password = "test-password";
    private SystemRole.RoleType _systemRole = SystemRole.RoleType.User;
    private Guid? _projectId;
    private ProjectRole.RoleType _projectRole = ProjectRole.RoleType.Member;

    public UserTestDataBuilder(IPasswordHashService passwordHashService)
    {
        _passwordHashService = passwordHashService;
    }

    public UserTestDataBuilder WithName(string name) { _name = name; return this; }
    public UserTestDataBuilder WithEmail(string email) { _email = email; return this; }
    public UserTestDataBuilder WithPassword(string password) { _password = password; return this; }
    public UserTestDataBuilder WithSystemRole(SystemRole.RoleType role) { _systemRole = role; return this; }
    public UserTestDataBuilder InProject(Guid projectId, ProjectRole.RoleType projectRole)
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
