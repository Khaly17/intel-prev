using System;
using System.Collections.Generic;
using MediatR;
using Soditech.IntelPrev.Prevensions.Shared.Equipments;
using Soditech.IntelPrev.Prevensions.Shared.Floors;

namespace Soditech.IntelPrev.Prevensions.Shared.Buildings;

public record BuildingCreatedEvent : INotification
{
    public Guid Id { get; set; }
    public bool IsDeleted { get; set; }
    
    public string Name { get; init; } = string.Empty;
    public string Address { get; init; } = string.Empty;
    public string Description { get; init; } = string.Empty;
    public int NumberOfFloors { get; init; }
    public bool HasDAE { get; init; }
    public bool HasFirstAidKits { get; init; }

    public List<FloorCreatedEvent> Floors { get; set; } = [];

    public List<EquipmentCreatedEvent> Equipments { get; set; } = [];

    public Guid TenantId { get; set; }

    public Guid? CreatorId { get; set; }
    public DateTimeOffset? CreatedAt { get; set; }
}

public record BuildingUpdatedEvent : INotification
{
    public Guid Id { get; set; }
    public bool IsDeleted { get; set; }
    
    public string Name { get; init; } = string.Empty;
    public string Address { get; init; } = string.Empty;
    public string Description { get; init; } = string.Empty;
    public int NumberOfFloors { get; init; }
    public bool HasDAE { get; init; }
    public bool HasFirstAidKits { get; init; }

    public List<FloorUpdatedEvent> Floors { get; set; } = [];

    public List<EquipmentUpdatedEvent> Equipments { get; set; } = [];

    public Guid TenantId { get; set; }
    
    public Guid? CreatorId { get; set; }
    public DateTimeOffset? CreatedAt { get; set; }


    public Guid? DeleterId { get; set; }
    public DateTimeOffset? DeletedAt { get; set; }
    
    public Guid? UpdaterId { get; set; }
    public DateTimeOffset? UpdatedAt { get; set; }
}

public record BuildingDeletedEvent(Guid Id): INotification;