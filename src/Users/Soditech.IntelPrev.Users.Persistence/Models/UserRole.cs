using System;
using Microsoft.AspNetCore.Identity;
using Sensor6ty.Domain;

namespace Soditech.IntelPrev.Users.Persistence.Models;

public class UserRole: IdentityUserRole<Guid>, IEntityBase
{
    public Guid Id { get; set; }
    
    public Guid? TenantId { get; set; }
    public virtual Tenant? Tenant { get; set; }

    public virtual User User { get; set; } = default!;
    public virtual Role Role { get; set; } = default!;

    /// <inheritdoc />
    public bool IsDeleted { get; set; }
}