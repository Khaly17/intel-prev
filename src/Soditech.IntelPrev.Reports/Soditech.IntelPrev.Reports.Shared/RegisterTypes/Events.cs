using System;
using System.Collections.Generic;
using MediatR;
using Soditech.IntelPrev.Reports.Shared.RegisterFields;
using Soditech.IntelPrev.Reports.Shared.RegisterFielGroups;

namespace Soditech.IntelPrev.Reports.Shared.RegisterTypes;

public record RegisterTypeCreatedEvent: INotification
{
    public Guid Id { get; set; }
    public bool IsDeleted { get; set; }
    
    public string Name { get; set; } = string.Empty;  // Type name (ex. SAVDHAS, Incident)
    public string Description { get; set; } = string.Empty; 
    public bool IsActive { get; set; }
    
    public List<RegisterFieldResult> RegisterFields { get; set; } = default!; // list of specifics fields
    public List<RegisterFieldGroupResult> RegisterFieldGroups { get; set; } = default!; 

    
    public Guid? CreatorId { get; set; }
    public DateTimeOffset? CreatedAt { get; set; }
}

public record RegisterTypeUpdatedEvent: INotification
{
    public Guid Id { get; set; }
    public bool IsDeleted { get; set; }
    
    public string Name { get; set; } = string.Empty;  // Type name (ex. SAVDHAS, Incident)
    public string Description { get; set; } = string.Empty; 
    public bool IsActive { get; set; }
    
    public List<RegisterFieldResult> RegisterFields { get; set; } = default!; // list of specifics fields
    public List<RegisterFieldGroupResult> RegisterFieldGroups { get; set; } = default!; 

    
    public Guid? CreatorId { get; set; }
    public DateTimeOffset? CreatedAt { get; set; }
    
    public Guid? DeleterId { get; set; }
    public DateTimeOffset? DeletedAt { get; set; }
    
    public Guid? UpdaterId { get; set; }
    public DateTimeOffset? UpdatedAt { get; set; }
}

public record RegisterTypeDeletedEvent(Guid Id): INotification ;