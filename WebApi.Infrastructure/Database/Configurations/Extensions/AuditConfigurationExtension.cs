using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WebApi.Domain.Common;

namespace WebApi.Infrastructure.Database.Configurations.Extensions;

public static class AuditConfigurationExtensions
{
    public static void OwnsAuditInfo<T>(this OwnedNavigationBuilder<T, AuditInfo> builder)
        where T : class
    {
        builder.Property(a => a.CreatedBy).HasColumnName("created_by").IsRequired();
        builder.Property(a => a.CreatedAt).HasColumnName("created_at").IsRequired();
        builder.Property(a => a.UpdatedBy).HasColumnName("updated_by").IsRequired();
        builder.Property(a => a.UpdatedAt).HasColumnName("updated_at").IsRequired();
    }
}
