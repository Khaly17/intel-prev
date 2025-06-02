using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Soditech.IntelPrev.Users.Persistence.Models;

namespace Soditech.IntelPrev.Users.Persistence.Configurations;

public class RoleConfiguration : IEntityTypeConfiguration<Role>
{
    /// <inheritdoc />
    public void Configure(EntityTypeBuilder<Role> builder)
    {
        builder.ToTable("Roles");

        builder.Property(r => r.Description)
            .HasMaxLength(100);

        builder.HasOne(r => r.Tenant)
            .WithMany(t => t.Roles)
            .HasForeignKey(r => r.TenantId)
            .IsRequired(false)
            .OnDelete(DeleteBehavior.NoAction);
        
        builder.Property(a => a.IsDefault)
            .HasDefaultValue(false);
        
        builder.Property(a => a.IsStatic)
            .HasDefaultValue(false);
        
        builder.Property(a => a.IsDeleted)
            .HasDefaultValue(false);
        
        builder.HasOne(r => r.Creator)
            .WithMany()
            .HasForeignKey(r => r.CreatorId)
            .IsRequired(false)
            .OnDelete(DeleteBehavior.NoAction);
        
        builder.HasMany(r => r.Users)
            .WithMany(u => u.Roles)
            .UsingEntity<UserRole>(
                r => r.HasOne<User>().WithMany().HasForeignKey(ur => ur.UserId),
                l => l.HasOne<Role>().WithMany().HasForeignKey(ur => ur.RoleId)
            );
        
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