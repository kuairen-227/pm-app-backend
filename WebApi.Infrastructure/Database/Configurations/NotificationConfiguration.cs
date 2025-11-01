using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WebApi.Domain.Aggregates.NotificationAggregate;

namespace WebApi.Infrastructure.Database.Configurations;

public class NotificationConfiguration : IEntityTypeConfiguration<Notification>
{
    public void Configure(EntityTypeBuilder<Notification> builder)
    {
        builder.OwnsOne(n => n.Category, category =>
        {
            category.Property(c => c.Value)
                .HasColumnName("category")
                .IsRequired();
        });
    }
}
