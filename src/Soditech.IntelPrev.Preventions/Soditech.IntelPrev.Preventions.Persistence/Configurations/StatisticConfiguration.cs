using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Soditech.IntelPrev.Preventions.Persistence.Models;

namespace Soditech.IntelPrev.Preventions.Persistence.Configurations;

public class StatisticConfiguration : IEntityTypeConfiguration<Statistic>
{
    /// <inheritdoc />
    public void Configure(EntityTypeBuilder<Statistic> builder)
    {
        builder.ToTable("Statistics");

        builder.Property(x => x.Category).HasMaxLength(100).IsRequired();
        builder.Property(x => x.Notes).HasMaxLength(500);
        builder.Property(x => x.Value).IsRequired();
        builder.Property(x => x.Date).IsRequired();
        
        builder.Property(x => x.IsDeleted).HasDefaultValue(false);
        builder.HasQueryFilter(x => !x.IsDeleted);
        
        builder.HasOne(u => u.Tenant)
            .WithMany()
            .HasForeignKey(u => u.TenantId)
            .IsRequired()
            .OnDelete(DeleteBehavior.NoAction);
        
        builder.HasOne(u => u.Campaign)
            .WithMany()
            .HasForeignKey(u => u.CampaignId)
            .IsRequired()
            .OnDelete(DeleteBehavior.NoAction);
    }
}