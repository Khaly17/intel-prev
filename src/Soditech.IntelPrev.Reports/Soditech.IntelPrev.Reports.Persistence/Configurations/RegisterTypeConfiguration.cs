using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Soditech.IntelPrev.Reports.Persistence.Models;

namespace Soditech.IntelPrev.Reports.Persistence.Configurations;

public class RegisterTypeConfiguration : IEntityTypeConfiguration<RegisterType>
{
    /// <inheritdoc />
    public void Configure(EntityTypeBuilder<RegisterType> builder)
    {
        builder.ToTable("RegisterTypes");
        builder.Property(x => x.Name).HasMaxLength(255).IsRequired();
        builder.Property(x => x.DisplayName).HasMaxLength(255)
            .HasDefaultValue("Set the default value.").IsRequired();
        builder.Property(x => x.Description).HasMaxLength(512).IsRequired();
        builder.Property(x => x.IsActive).IsRequired();
        
        builder.Property(x => x.IsDeleted).HasDefaultValue(false);
        builder.HasQueryFilter(x => !x.IsDeleted);
        
        
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

        builder.HasMany(rt => rt.RegisterFields)
                .WithOne(rf => rf.RegisterType)
                .HasForeignKey(rf => rf.RegisterTypeId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(rt => rt.RegisterFieldGroups)
                .WithOne(rfg => rfg.RegisterType)
                .HasForeignKey(rfg => rfg.RegisterTypeId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);
    }
}