using System;
using System.Collections.Generic;
using Sensor6ty.Domain;

namespace Soditech.IntelPrev.Reports.Persistence.Models;

public class RegisterFieldGroup: EntityBase
{ 
    public string Name { get; set; }  = string.Empty; // Nom du groupe (ex. "Identité", "Détails")
    public string Description { get; set; }  = string.Empty; // Description du groupe
    public int DisplayOrder { get; set; } // Display order of the group
    
    public Guid RegisterTypeId { get; set; } // reference to the register type
    public virtual RegisterType RegisterType { get; set; } = null!;
    public virtual List<RegisterField> RegisterFields { get; set; } = null!;
    
    
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