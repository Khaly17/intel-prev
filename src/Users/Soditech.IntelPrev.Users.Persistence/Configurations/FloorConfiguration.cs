using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Soditech.IntelPrev.Users.Persistence.Models;

namespace Soditech.IntelPrev.Users.Persistence.Configurations;

public class FloorConfiguration : IEntityTypeConfiguration<Floor>
{
    /// <inheritdoc />
    public void Configure(EntityTypeBuilder<Floor> builder)
    {
        builder.ToTable("Floors");

        builder.Property(x => x.Number).HasMaxLength(100).IsRequired();

        
        builder.Property(x => x.IsDeleted).HasDefaultValue(false);
        builder.HasQueryFilter(x => !x.IsDeleted);
        
        builder.HasOne(u => u.Tenant)
            .WithMany()
            .HasForeignKey(u => u.TenantId)
            .IsRequired()
            .OnDelete(DeleteBehavior.NoAction);
        
        builder.HasOne(u => u.Building)
            .WithMany(b=>b.Floors)
            .HasForeignKey(u => u.BuildingId)
            .IsRequired()
            .OnDelete(DeleteBehavior.NoAction);
        
        builder.HasMany(u => u.Users)
            .WithOne(b=>b.Floor)
            .HasForeignKey(u => u.FloorId)
            .IsRequired(false)
            .OnDelete(DeleteBehavior.NoAction);
        
    }
}