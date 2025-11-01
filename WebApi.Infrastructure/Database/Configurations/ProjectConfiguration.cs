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
