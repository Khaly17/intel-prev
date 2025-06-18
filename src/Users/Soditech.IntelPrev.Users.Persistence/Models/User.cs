using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;
using Sensor6ty.Domain;

namespace Soditech.IntelPrev.Users.Persistence.Models;

public class User : IdentityUser<Guid>, IEntityBase
{
    public Guid? TenantId { get; set; }
    public virtual Tenant? Tenant { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    
    public string FullName => $"{FirstName} {LastName}";
    public string AppVersion { get; set; } = string.Empty;
    
    public virtual Building? Building { get; set; }
    public Guid? BuildingId { get; set; }

    public virtual Floor? Floor { get; set; }
    public Guid? FloorId { get; set; }


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
    public virtual List<Role> Roles { get; } = [];
    //public virtual List<UserRole> UserRoles { get; set; } = [];

    /// <inheritdoc />
    public bool IsDeleted { get; set; }
}