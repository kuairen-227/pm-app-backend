using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WebApi.Domain.Aggregates.ProjectAggregate;
using WebApi.Domain.Aggregates.UserAggregate;

namespace WebApi.Infrastructure.Database.Configurations;

public class ProjectConfiguration : IEntityTypeConfiguration<Project>
{
    public void Configure(EntityTypeBuilder<Project> builder)
    {
        builder.OwnsMany(p => p.Members, member =>
        {
            member.ToTable("project_members");
            member.Property(m => m.Id).ValueGeneratedOnAdd();
            member.HasKey(m => m.Id);

            member.HasOne<User>()
                .WithMany()
                .HasForeignKey(m => m.UserId)
                .IsRequired();
        });
    }
}
