using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Soditech.IntelPrev.Reports.Persistence.Models;
using Soditech.IntelPrev.Reports.Shared.Enums;

namespace Soditech.IntelPrev.Reports.Persistence.Configurations;

public class AlertConfiguration : IEntityTypeConfiguration<Alert>
{
    /// <inheritdoc />
    public void Configure(EntityTypeBuilder<Alert> builder)
    {
        builder.ToTable("Alerts");
        builder.Property(x => x.Title).HasMaxLength(64).IsRequired();
        builder.Property(x => x.Description).HasMaxLength(512).IsRequired();
        builder.Property(x => x.Type).IsRequired()
            .HasConversion(x => x.ToString(), s => Enum.Parse<AlertType>(s));
        builder.Property(x => x.Latitude).IsRequired();
        builder.Property(x => x.Longitude).IsRequired();
        
        builder.Property(x => x.IsDeleted).HasDefaultValue(false);
        builder.HasQueryFilter(x => !x.IsDeleted);


        builder.HasOne(u => u.Building)
            .WithMany()
            .HasForeignKey(u => u.BuildingId)
            .IsRequired()
            .OnDelete(DeleteBehavior.NoAction);
        
        builder.HasOne(u => u.Floor)
            .WithMany()
            .HasForeignKey(u => u.FloorId)
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