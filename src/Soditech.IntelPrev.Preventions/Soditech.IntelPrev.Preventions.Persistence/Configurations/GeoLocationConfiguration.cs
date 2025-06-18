using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Soditech.IntelPrev.Prevensions.Shared.Enums;
using Soditech.IntelPrev.Preventions.Persistence.Models;

namespace Soditech.IntelPrev.Preventions.Persistence.Configurations;

public class GeoLocationConfiguration : IEntityTypeConfiguration<GeoLocation>
{
    /// <inheritdoc />
    public void Configure(EntityTypeBuilder<GeoLocation> builder)
    {
        builder.ToTable("GeoLocations");

        builder.Property(x => x.Longitude).IsRequired();
        builder.Property(x => x.Latitude).IsRequired();
        builder.Property(x => x.Type).IsRequired()
            .HasConversion(t => t.ToString(), s => Enum.Parse<GeoLocationType>(s));

        
        builder.Property(x => x.IsDeleted).HasDefaultValue(false);
        builder.HasQueryFilter(x => !x.IsDeleted);
        
        
        builder.HasOne(u => u.Building)
            .WithMany(b=>b.GeoLocations)
            .HasForeignKey(u => u.BuildingId)
            .IsRequired()
            .OnDelete(DeleteBehavior.NoAction);
        
        builder.HasOne(u => u.Floor)
            .WithMany(b=>b.GeoLocations)
            .HasForeignKey(u => u.FloorId)
            .OnDelete(DeleteBehavior.NoAction);

        builder.HasOne(u => u.Equipment)
            .WithOne(b => b.GeoLocation)
            .HasForeignKey<GeoLocation>(e => e.EquipmentId)
            .OnDelete(DeleteBehavior.NoAction);

        builder.HasOne(u => u.Tenant)
            .WithMany()
            .HasForeignKey(u => u.TenantId)
            .IsRequired()
            .OnDelete(DeleteBehavior.NoAction);
    }
}