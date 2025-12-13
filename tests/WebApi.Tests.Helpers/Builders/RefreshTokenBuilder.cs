using WebApi.Domain.Aggregates.AuthAggregate;
using WebApi.Tests.Helpers.Builders.Common;

namespace WebApi.Tests.Helpers.Builders;

public class RefreshTokenBuilder : BaseBuilder<RefreshTokenBuilder, RefreshToken>
{
    private Guid _userId = Guid.NewGuid();
    private string _token = "default_token";
    private DateTime _expiresAt = DateTime.UtcNow.AddDays(7);
    private bool _isRevoked = false;

    public RefreshTokenBuilder WithUserId(Guid userId)
    {
        _userId = userId;
        return this;
    }

    public RefreshTokenBuilder WithToken(string token)
    {
        _token = token;
        return this;
    }

    public RefreshTokenBuilder WithExpiresAt(DateTime expiresAt)
    {
        _expiresAt = expiresAt;
        return this;
    }

    public RefreshTokenBuilder WithIsRevoked(bool isRevoked)
    {
        _isRevoked = isRevoked;
        return this;
    }

    public override RefreshToken Build()
    {
        var refreshToken = new RefreshToken(
            _userId,
            _token,
            _expiresAt,
            _createdBy,
            _clock
        );

        if (_isRevoked)
        {
            refreshToken.Revoke(_userId, _clock);
        }

        return refreshToken;
    }
}
