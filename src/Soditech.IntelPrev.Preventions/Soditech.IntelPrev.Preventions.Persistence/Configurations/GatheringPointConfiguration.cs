using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Soditech.IntelPrev.Preventions.Persistence.Models;

namespace Soditech.IntelPrev.Preventions.Persistence.Configurations;

public class GatheringPointConfiguration : IEntityTypeConfiguration<GatheringPoint>
{
    /// <inheritdoc />
    public void Configure(EntityTypeBuilder<GatheringPoint> builder)
    {
        builder.ToTable("GatheringPoints");

        builder.Property(x => x.Name).HasMaxLength(100).IsRequired();
        
        builder.Property(x => x.IsDeleted).HasDefaultValue(false);
        builder.HasQueryFilter(x => !x.IsDeleted);
        
        builder.HasOne(u => u.Tenant)
            .WithMany()
            .HasForeignKey(u => u.TenantId)
            .IsRequired()
            .OnDelete(DeleteBehavior.NoAction);
        
        builder.HasOne(u => u.Building)
            .WithMany(b=>b.GatheringPoints)
            .HasForeignKey(u => u.BuildingId)
            .IsRequired()
            .OnDelete(DeleteBehavior.NoAction);
        
        builder.HasOne(u => u.Floor)
            .WithMany(b=>b.GatheringPoints)
            .HasForeignKey(u => u.FloorId)
            .OnDelete(DeleteBehavior.NoAction);
        
        builder.HasOne(u => u.GeoLocation)
            .WithOne(b=>b.GatheringPoint)
            .HasForeignKey<GatheringPoint>(e=> e.GeoLocationId)
            .OnDelete(DeleteBehavior.NoAction);
    }
}