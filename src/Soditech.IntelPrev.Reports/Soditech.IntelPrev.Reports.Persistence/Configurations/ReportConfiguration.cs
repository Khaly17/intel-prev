using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Soditech.IntelPrev.Reports.Persistence.Models;
using Soditech.IntelPrev.Reports.Shared.Enums;

namespace Soditech.IntelPrev.Reports.Persistence.Configurations;

public class ReportConfiguration : IEntityTypeConfiguration<Report>
{
    /// <inheritdoc />
    public void Configure(EntityTypeBuilder<Report> builder)
    {
        builder.ToTable("Reports");
        builder.Property(x => x.Title).HasMaxLength(255).IsRequired();
        builder.Property(x => x.DisplayName).HasMaxLength(255)
            .HasDefaultValue("Set the default value.").IsRequired();
        builder.Property(x => x.Description).HasMaxLength(512).IsRequired();
        builder.Property(x => x.Status).IsRequired()
            .HasConversion(x => x.ToString(), s => Enum.Parse<ReportStatus>(s));
        
        builder.Property(x => x.IsDeleted).HasDefaultValue(false);
        builder.HasQueryFilter(x => !x.IsDeleted);
        
        
        builder.HasOne(u => u.RegisterType)
            .WithMany()
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