using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WebApi.Domain.Aggregates.TicketAggregate;

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

        builder.OwnsMany(t => t.AssignmentHistories, history =>
        {
            history.ToTable("assignment_histories");
            history.Property(h => h.Id).ValueGeneratedOnAdd();
            history.HasKey(h => h.Id);
        });
    }
}
