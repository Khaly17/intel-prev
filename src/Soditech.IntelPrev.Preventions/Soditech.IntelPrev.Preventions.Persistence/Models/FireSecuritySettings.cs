using Sensor6ty.Domain;

namespace Soditech.IntelPrev.Preventions.Persistence.Models;

public class FireSecuritySettings : EntityBase
{
    public string DefinitionContent { get; set; } = string.Empty;
    public string KnownMyEnterpriseContent { get; set; } = string.Empty;
    public string FireSecurityServiceContent { get; set; } = string.Empty;
    public string FireConsignsContent { get; set; } = string.Empty;
    public string FireMaterialsContent { get; set; } = string.Empty;
    public string EvacuationCaseContent { get; set; } = string.Empty;
    public bool IsDefault { get; set; }
    
    public Guid? TenantId { get; set; }
    public virtual Tenant? Tenant { get; set; } = default!;
    
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