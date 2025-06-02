using Sensor6ty.Domain;

namespace Soditech.IntelPrev.Preventions.Persistence.Models;

public class Floor : EntityBase
{
    public int Number { get; set; }
    public virtual List<Equipment> Equipments { get; set; } = default!;

    public virtual List<GeoLocation> GeoLocations { get; set; } = default!;
    public virtual List<GatheringPoint> GatheringPoints { get; set; } = default!;

    public Guid BuildingId { get; set; }
    public virtual Building Building { get; set; } = default!;


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