using System;
using Sensor6ty.Domain;
using Soditech.IntelPrev.Prevensions.Shared.Enums;

namespace Soditech.IntelPrev.Preventions.Persistence.Models;

public class GeoLocation : EntityBase
{
    public double Latitude { get; set; }
    public double Longitude { get; set; }
    public double? Altitude { get; set; }

    //is it a building, floor or equipment
    public GeoLocationType Type { get; set; } 

    public Guid BuildingId { get; set; }
    public virtual Building Building { get; set; } = default!;


    public Guid? FloorId { get; set; }
    public virtual Floor? Floor { get; set; }
    
    public Guid? EquipmentId { get; set; }
    public virtual Equipment? Equipment { get; set; }

     
    public Guid? GatheringPointId { get; set; }
    public virtual GatheringPoint? GatheringPoint { get; set; }


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

