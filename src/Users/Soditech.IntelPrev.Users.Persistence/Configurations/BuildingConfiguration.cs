using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Soditech.IntelPrev.Users.Persistence.Models;

namespace Soditech.IntelPrev.Users.Persistence.Configurations;

public class BuildingConfiguration : IEntityTypeConfiguration<Building>
{
    /// <inheritdoc />
    public void Configure(EntityTypeBuilder<Building> builder)
    {
        builder.ToTable("Buildings");

        builder.Property(x => x.Name).HasMaxLength(100).IsRequired();
        builder.Property(x => x.Address).HasMaxLength(500);
        builder.Property(x => x.Description).HasMaxLength(500);
        
        builder.Property(x => x.IsDeleted).HasDefaultValue(false);
        builder.HasQueryFilter(x => !x.IsDeleted);
        
        builder.HasOne(u => u.Tenant)
            .WithMany()
            .HasForeignKey(u => u.TenantId)
            .IsRequired()
            .OnDelete(DeleteBehavior.NoAction);

        builder.HasMany(u => u.Floors)
            .WithOne(b => b.Building)
            .HasForeignKey(u => u.BuildingId)
            .IsRequired()
            .OnDelete(DeleteBehavior.NoAction);
        
        builder.HasMany(u => u.Users)
            .WithOne(b => b.Building)
            .HasForeignKey(u => u.BuildingId)
            .IsRequired(false)
            .OnDelete(DeleteBehavior.NoAction);

    }
}