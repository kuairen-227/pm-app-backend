using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WebApi.Domain.Aggregates.AuthAggregate;
using WebApi.Domain.Aggregates.UserAggregate;
using WebApi.Infrastructure.Database.Configurations.Extensions;

namespace WebApi.Infrastructure.Database.Configurations;

public class RefreshTokenConfiguration : IEntityTypeConfiguration<RefreshToken>
{
    public void Configure(EntityTypeBuilder<RefreshToken> builder)
    {
        builder.Property(rt => rt.Token)
            .HasColumnName("token")
            .HasMaxLength(256)
            .IsRequired();

        builder.OwnsOne(rt => rt.AuditInfo, a =>
        {
            a.OwnsAuditInfo();
        });

        builder.HasOne<User>()
            .WithMany()
            .HasForeignKey(rt => rt.UserId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
