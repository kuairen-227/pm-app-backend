using WebApi.Domain.Abstractions;
using WebApi.Domain.Common;

namespace WebApi.Domain.Aggregates.AuthAggregate;

public class RefreshToken : Entity
{
    public Guid UserId { get; set; }
    public string Token { get; set; } = default!;
    public DateTime ExpiresAt { get; set; }
    public bool IsRevoked { get; set; }
    public DateTime? RevokedAt { get; set; }

    private RefreshToken() { } // EF Core 用

    public RefreshToken(Guid userId, string token, DateTime expiresAt, Guid createdBy, IDateTimeProvider clock)
        : base(createdBy, clock)
    {
        if (string.IsNullOrWhiteSpace(token))
            throw new DomainException("REFRESH_TOKEN_REQUIRED", "Token は必須です");
        if (expiresAt < clock.Now)
            throw new DomainException("REFRESH_TOKEN_INVALID_EXPIRY", "ExpiresAt は現在時刻より後である必要があります");

        UserId = userId;
        Token = token;
        ExpiresAt = expiresAt;
        IsRevoked = false;
    }

    public void Revoke(Guid revokedBy, IDateTimeProvider clock)
    {
        if (IsRevoked)
            throw new DomainException("REFRESH_TOKEN_ALREADY_REVOKED", "このリフレッシュトークンは既に取り消されています");

        IsRevoked = true;
        RevokedAt = clock.Now;
        UpdateAuditInfo(revokedBy);
    }
}
