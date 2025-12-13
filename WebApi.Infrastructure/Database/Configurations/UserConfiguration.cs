using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WebApi.Domain.Aggregates.UserAggregate;
using WebApi.Infrastructure.Database.Configurations.Extensions;

namespace WebApi.Infrastructure.Database.Configurations;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.Property(u => u.Name)
            .HasColumnName("name")
            .HasMaxLength(100)
            .IsRequired();

        builder.OwnsOne(u => u.Email, email =>
        {
            email.Property(e => e.Value)
                .HasColumnName("email")
                .HasMaxLength(254)  // RFC 5321 / 5322
                .IsRequired();
        });

        builder.Property(u => u.PasswordHash)
            .HasColumnName("password_hash")
            .HasMaxLength(255)
            .IsRequired();

        builder.OwnsOne(u => u.Role, role =>
        {
            role.Property(r => r.Value)
                .HasColumnName("role")
                .IsRequired();
        });

        builder.OwnsOne(u => u.AuditInfo, a =>
        {
            a.OwnsAuditInfo();
        });
    }
}
