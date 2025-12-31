using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
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
                .HasMaxLength(100)
                .IsRequired();
        });

        builder.OwnsOne(t => t.Description, description =>
        {
            description.Property(d => d.Value)
                .HasColumnName("description")
                .IsRequired();
        });

        builder.OwnsOne(t => t.Schedule, schedule =>
        {
            schedule.Property(s => s.StartDate)
                .HasColumnName("start_date");
            schedule.Property(s => s.EndDate)
                .HasColumnName("end_date");
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

        builder.OwnsMany(t => t.CompletionCriteria, criteria =>
        {
            criteria.ToTable("ticket_completion_criteria");
            criteria.Property(c => c.Criterion)
                .HasMaxLength(200);
            criteria.OwnsOne(c => c.AuditInfo, a =>
            {
                a.OwnsAuditInfo();
            });
        });

        builder.OwnsMany(t => t.Comments, comment =>
        {
            comment.ToTable("ticket_comments");
            comment.OwnsOne(c => c.AuditInfo, a =>
            {
                a.OwnsAuditInfo();
            });
        });

        builder.OwnsMany(t => t.Histories, history =>
        {
            history.ToTable("ticket_histories");
            history.Property(h => h.Action)
                .HasConversion<string>()
                .HasMaxLength(50);
            history.Property(h => h.Changes)
                .HasConversion(new TicketHistoryChangesJsonConverter())
                .HasColumnType("jsonb");
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
