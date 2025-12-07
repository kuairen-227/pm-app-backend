using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WebApi.Infrastructure.Database.Configurations.Extensions;
using WebApi.Infrastructure.Database.Entities;

namespace WebApi.Infrastructure.Database.Configurations;

public class RefreshTokenConfiguration : IEntityTypeConfiguration<RefreshToken>
{
    public void Configure(EntityTypeBuilder<RefreshToken> builder)
    {
        builder.OwnsOne(rt => rt.AuditInfo, a =>
        {
            a.OwnsAuditInfo();
        });
    }
}
