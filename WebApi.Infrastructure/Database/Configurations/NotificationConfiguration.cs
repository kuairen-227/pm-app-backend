using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WebApi.Domain.Aggregates.NotificationAggregate;
using WebApi.Domain.Aggregates.UserAggregate;
using WebApi.Infrastructure.Database.Configurations.Extensions;

namespace WebApi.Infrastructure.Database.Configurations;

public class NotificationConfiguration : IEntityTypeConfiguration<Notification>
{
    public void Configure(EntityTypeBuilder<Notification> builder)
    {
        builder.Property(n => n.RecipientId)
            .HasColumnName("user_id");

        builder.OwnsOne(n => n.Category, category =>
        {
            category.Property(c => c.Value)
                .HasColumnName("category")
                .IsRequired();
        });

        builder.OwnsOne(n => n.AuditInfo, a =>
        {
            a.OwnsAuditInfo();
        });

        builder.HasOne<User>()
            .WithMany()
            .HasForeignKey(n => n.RecipientId)
            .IsRequired();
    }
}
