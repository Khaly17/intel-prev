using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Soditech.IntelPrev.Users.Persistence.Models;

namespace Soditech.IntelPrev.Users.Persistence.Configurations;

public class TenantConfiguration : IEntityTypeConfiguration<Tenant> 
{
    /// <inheritdoc />
    public void Configure(EntityTypeBuilder<Tenant> builder)
    {
        builder.ToTable("Tenants");

        builder.Property(x => x.Name)
            .HasMaxLength(100);

        builder.Property(x => x.DisplayName)
            .HasMaxLength(150);
		

        builder.Property(a => a.IsActive)
            .HasDefaultValue(true);
        
        builder.Property(a => a.IsDeleted)
            .HasDefaultValue(false);
        
        builder.HasOne(r => r.Creator)
            .WithMany()
            .HasForeignKey(r => r.CreatorId)
            .IsRequired(false)
            .OnDelete(DeleteBehavior.NoAction);
        
        builder.HasOne(r => r.Updater)
            .WithMany()
            .HasForeignKey(r => r.UpdaterId)
            .IsRequired(false)
            .OnDelete(DeleteBehavior.NoAction);
        
        builder.HasOne(r => r.Deleter)
            .WithMany()
            .HasForeignKey(r => r.DeleterId)
            .IsRequired(false)
            .OnDelete(DeleteBehavior.NoAction);
    }
}