using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Soditech.IntelPrev.Reports.Persistence.Models;

namespace Soditech.IntelPrev.Reports.Persistence.Configurations;

public class RegisterFieldGroupConfiguration : IEntityTypeConfiguration<RegisterFieldGroup>
{
    /// <inheritdoc />
    public void Configure(EntityTypeBuilder<RegisterFieldGroup> builder)
    {
        builder.ToTable("RegisterFieldGroups");
        builder.Property(x => x.Name).HasMaxLength(255).IsRequired();
        builder.Property(x => x.Description).HasMaxLength(512).IsRequired(false);
        builder.Property(x => x.DisplayOrder).IsRequired();
        
        builder.Property(x => x.IsDeleted).HasDefaultValue(false);
        builder.HasQueryFilter(x => !x.IsDeleted);
        
        
        builder.HasOne(u => u.RegisterType)
            .WithMany(x => x.RegisterFieldGroups)
            .HasForeignKey(u => u.RegisterTypeId)
            .IsRequired()
            .OnDelete(DeleteBehavior.NoAction);
        
        builder.HasOne(u => u.Tenant)
            .WithMany()
            .HasForeignKey(u => u.TenantId)
            .IsRequired()
            .OnDelete(DeleteBehavior.NoAction);
        
        
        builder.HasOne(u => u.Creator)
            .WithMany()
            .HasForeignKey(u => u.CreatorId)
            .IsRequired(false)
            .OnDelete(DeleteBehavior.NoAction);
        
        builder.HasOne(u => u.Updater)
            .WithMany()
            .HasForeignKey(u => u.UpdaterId)
            .IsRequired(false)
            .OnDelete(DeleteBehavior.NoAction);
        
        builder.HasOne(u => u.Deleter)
            .WithMany()
            .HasForeignKey(u => u.DeleterId)
            .IsRequired(false)
            .OnDelete(DeleteBehavior.NoAction);
    }
}