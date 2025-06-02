using Sensor6ty.Domain;
using Soditech.IntelPrev.Reports.Shared.Enums;

namespace Soditech.IntelPrev.Reports.Persistence.Models;

public class RegisterField: EntityBase
{
    public string Name { get; set; } = string.Empty;  // Field name (ex. Victime, Lieu)
    public string Description { get; set; } = string.Empty; // Field description
    public FieldType FieldType { get; set; } // Field type (texte, date, booléen, etc.)
    public bool IsRequired { get; set; } // Required or not
    
    public int DisplayOrder { get; set; } // Display order
    
    public Guid? RegisterFieldGroupId { get; set; } // reference to the register field group
    public virtual RegisterFieldGroup? RegisterFieldGroup { get; set; } = default!;
    
    public Guid RegisterTypeId { get; set; } // reference to the register type
    public virtual RegisterType RegisterType { get; set; } = default!;
    
    public Guid TenantId { get; set; }
    public virtual Tenant Tenant { get; set; } = default!;
    
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