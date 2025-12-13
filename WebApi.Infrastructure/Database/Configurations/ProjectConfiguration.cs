using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WebApi.Domain.Aggregates.ProjectAggregate;
using WebApi.Domain.Aggregates.UserAggregate;
using WebApi.Infrastructure.Database.Configurations.Extensions;

namespace WebApi.Infrastructure.Database.Configurations;

public class ProjectConfiguration : IEntityTypeConfiguration<Project>
{
    public void Configure(EntityTypeBuilder<Project> builder)
    {
        builder.Property(p => p.Name)
            .HasColumnName("name")
            .HasMaxLength(200)
            .IsRequired();

        builder.Property(p => p.Description)
            .HasColumnName("description")
            .HasMaxLength(1000);

        builder.OwnsOne(p => p.AuditInfo, a =>
        {
            a.OwnsAuditInfo();
        });

        builder.OwnsMany(p => p.Members, member =>
        {
            member.OwnsOne(m => m.Role, role =>
            {
                role.Property(r => r.Value)
                    .HasColumnName("role");
            });
            member.OwnsOne(m => m.AuditInfo, a =>
            {
                a.OwnsAuditInfo();
            });
            member.HasOne<User>()
                .WithMany()
                .HasForeignKey(m => m.UserId)
                .IsRequired();
        });
    }
}
