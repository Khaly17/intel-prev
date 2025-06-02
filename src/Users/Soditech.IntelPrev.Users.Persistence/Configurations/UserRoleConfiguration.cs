using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Soditech.IntelPrev.Users.Persistence.Models;

namespace Soditech.IntelPrev.Users.Persistence.Configurations;

public class UserRoleConfiguration : IEntityTypeConfiguration<UserRole>
{
    /// <inheritdoc />
    public void Configure(EntityTypeBuilder<UserRole> builder)
    {
        builder.ToTable("UserRoles");

        builder.HasKey(ur => ur.Id);

        builder.HasOne(ur => ur.Role)
            // .WithMany(r => r.UserRoles)
            .WithMany()
            .HasForeignKey(ur => ur.RoleId)
            .IsRequired()
            .OnDelete(DeleteBehavior.NoAction);

        builder.HasOne(ur => ur.User)
            // .WithMany(u=> u.UserRoles)
            .WithMany()
            .HasForeignKey(ur => ur.UserId)
            .IsRequired()
            .OnDelete(DeleteBehavior.NoAction);

        builder.HasOne(ur => ur.Tenant)
            .WithMany()
            .HasForeignKey(ur => ur.TenantId)
            .IsRequired(false)
            .OnDelete(DeleteBehavior.NoAction);

        builder.HasIndex(ur => new { ur.UserId, ur.RoleId, ur.TenantId })
            .IsUnique();

        builder.Property(a => a.IsDeleted)
            .HasDefaultValue(false);

    }
}