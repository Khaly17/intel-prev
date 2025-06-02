using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Soditech.IntelPrev.Preventions.Persistence.Models;

namespace Soditech.IntelPrev.Preventions.Persistence.Configurations;

public class StaticContentConfiguration : IEntityTypeConfiguration<StaticContent>
{
    /// <inheritdoc />
    public void Configure(EntityTypeBuilder<StaticContent> builder)
    {
        builder.ToTable("StaticContents");

        builder.Property(x => x.Key).HasMaxLength(100).IsRequired();
        builder.Property(x => x.Title).HasMaxLength(500);
        builder.Property(x => x.Content).IsRequired();
        
        builder.Property(x => x.IsDeleted).HasDefaultValue(false);
        builder.HasQueryFilter(x => !x.IsDeleted);
        
        builder.HasOne(u => u.Tenant)
            .WithMany()
            .HasForeignKey(u => u.TenantId)
            .IsRequired()
            .OnDelete(DeleteBehavior.NoAction);
    }
}