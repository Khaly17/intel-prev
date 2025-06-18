using System;
using System.Collections.Generic;
using Sensor6ty.Domain;

namespace Soditech.IntelPrev.Reports.Persistence.Models;

public class RegisterType: EntityBase
{
    public string Name { get; set; } = string.Empty;  // Type name (ex. SAVDHAS, Incident)
    public string DisplayName { get; set; } = string.Empty;  // Type name (ex. SAVDHAS, Incident)
    public string Description { get; set; } = string.Empty; 
    public bool IsActive { get; set; }
    
    public virtual List<RegisterField> RegisterFields { get; set; } = null!; // list of specifics fields
    public virtual List<RegisterFieldGroup> RegisterFieldGroups { get; set; } = null!; // list of specifics field groups

    public Guid TenantId { get; set; }
    public virtual Tenant Tenant { get; set; } = null!;
    
    public virtual User? Creator { get; set; }
    public Guid? CreatorId { get; set; }
    public DateTimeOffset? CreatedAt { get; set; }

    public virtual User? Deleter { get; set; }
    public Guid? DeleterId { get; set; }
    public DateTimeOffset? DeletedAt { get; set; }

    public virtual User? Updater { get; set; }
    public Guid? UpdaterId { get; set; }
    public DateTimeOffset? UpdatedAt { get; set; }
}