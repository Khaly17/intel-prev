using Sensor6ty.Domain;
using Soditech.IntelPrev.Prevensions.Shared.Enums;

namespace Soditech.IntelPrev.Preventions.Persistence.Models;

public class Equipment : EntityBase
{
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public EquipmentType Type { get; set; } 
    public DateTimeOffset LastInspectionDate { get; set; }
    public DateTimeOffset NextInspectionDate { get; set; }
    
    public Guid BuildingId { get; set; }
    public virtual Building Building { get; set; } = default!;

    //the location of the equipment may be unknown when creating it.
    public Guid? GeoLocationId { get; set; }
    public virtual GeoLocation? GeoLocation { get; set; } = default!;
    

    public Guid? FloorId { get; set; }
    public virtual Floor? Floor { get; set; } = default!;
    
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