using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WebApi.Domain.Aggregates.UserAggregate;
using WebApi.Infrastructure.Database.Configurations.Extensions;

namespace WebApi.Infrastructure.Database.Configurations;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.OwnsOne(u => u.Email, email =>
        {
            email.Property(e => e.Value)
                .HasColumnName("email")
                .IsRequired();
        });

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
