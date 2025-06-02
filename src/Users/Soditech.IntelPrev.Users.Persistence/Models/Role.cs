using Microsoft.AspNetCore.Identity;
using Sensor6ty.Domain;

namespace Soditech.IntelPrev.Users.Persistence.Models;

public class Role : IdentityRole<Guid>, IEntityBase
{
    public string Description { get; set; } = string.Empty;
    
    public Guid? TenantId { get; set; }
    public virtual Tenant? Tenant { get; set; }
    
    public bool IsDefault { get; set; }
    /// <summary>
    /// Static roles are created by the system and cannot be deleted or modified.
    /// </summary>
    public bool IsStatic { get; set; }
    
    public virtual User? Creator { get; set; }
    public Guid? CreatorId { get; set; }
    public DateTimeOffset? CreatedAt { get; set; }
    
    public virtual User? Deleter { get; set; }
    public Guid? DeleterId { get; set; }
    public DateTimeOffset? DeletedAt { get; set; }
    
    public virtual User? Updater { get; set; }
    public Guid? UpdaterId { get; set; }
    public DateTimeOffset? UpdatedAt { get; set; }
    
    
    // user roles
    public virtual List<User> Users { get; set; } = [];

    /// <inheritdoc />
    public bool IsDeleted { get; set; }
}