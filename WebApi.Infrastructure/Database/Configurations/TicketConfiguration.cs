using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WebApi.Domain.Aggregates.ProjectAggregate;
using WebApi.Domain.Aggregates.TicketAggregate;
using WebApi.Domain.Aggregates.UserAggregate;
using WebApi.Infrastructure.Database.Configurations.Extensions;

namespace WebApi.Infrastructure.Database.Configurations;

public class TicketConfiguration : IEntityTypeConfiguration<Ticket>
{
    public void Configure(EntityTypeBuilder<Ticket> builder)
    {
        builder.OwnsOne(t => t.Title, title =>
        {
            title.Property(t => t.Value)
                .HasColumnName("title")
                .IsRequired();
        });

        builder.OwnsOne(t => t.Deadline, deadline =>
        {
            deadline.Property(d => d.Value)
                .HasColumnName("deadline");
        });

        builder.OwnsOne(t => t.Status, status =>
        {
            status.Property(s => s.Value)
                .HasColumnName("status")
                .IsRequired();
        });

        builder.OwnsOne(n => n.AuditInfo, a =>
        {
            a.OwnsAuditInfo();
        });

        builder.OwnsMany(t => t.Comments, comment =>
        {
            comment.ToTable("ticket_comments");
            comment.OwnsOne(c => c.AuditInfo, a =>
            {
                a.OwnsAuditInfo();
            });
        });

        builder.OwnsMany(t => t.AssignmentHistories, history =>
        {
            history.ToTable("assignment_histories");
            history.Property(h => h.Id).ValueGeneratedOnAdd();
            history.OwnsOne(h => h.AuditInfo, a =>
            {
                a.OwnsAuditInfo();
            });
        });

        builder.HasOne<User>()
            .WithMany()
            .HasForeignKey(t => t.AssigneeId)
            .IsRequired();

        builder.HasOne<Project>()
            .WithMany()
            .HasForeignKey(t => t.ProjectId)
            .IsRequired();
    }
}
