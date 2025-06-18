using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Soditech.IntelPrev.Prevensions.Shared.Enums;
using Soditech.IntelPrev.Preventions.Persistence.Models;

namespace Soditech.IntelPrev.Preventions.Persistence.Configurations;

public class EquipmentConfiguration : IEntityTypeConfiguration<Equipment>
{
    /// <inheritdoc />
    public void Configure(EntityTypeBuilder<Equipment> builder)
    {
        builder.ToTable("Equipments");

        builder.Property(x => x.Name).HasMaxLength(100).IsRequired();
        builder.Property(x => x.Description).HasMaxLength(500);
        builder.Property(x => x.Type)
            .HasConversion(e => e.ToString(), s => Enum.Parse<EquipmentType>(s))
            .HasMaxLength(100);
        
        builder.Property(x => x.IsDeleted).HasDefaultValue(false);
        builder.HasQueryFilter(x => !x.IsDeleted);
        
        builder.HasOne(u => u.Tenant)
            .WithMany()
            .HasForeignKey(u => u.TenantId)
            .IsRequired()
            .OnDelete(DeleteBehavior.NoAction);
        
        builder.HasOne(u => u.Building)
            .WithMany(b=>b.Equipments)
            .HasForeignKey(u => u.BuildingId)
            .IsRequired()
            .OnDelete(DeleteBehavior.NoAction);
        
        builder.HasOne(u => u.Floor)
            .WithMany(b=>b.Equipments)
            .HasForeignKey(u => u.FloorId)
            .OnDelete(DeleteBehavior.NoAction);
        
        builder.HasOne(u => u.GeoLocation)
            .WithOne(b=>b.Equipment)
            .HasForeignKey<Equipment>(e=> e.GeoLocationId)
            .OnDelete(DeleteBehavior.NoAction);
    }
}