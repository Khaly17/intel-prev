using System;
using MediatR;

namespace Soditech.IntelPrev.Reports.Shared.RegisterFielGroups;

public record RegisterFieldGroupCreatedEvent : INotification
{
    public Guid Id { get; set; }
    public bool IsDeleted { get; set; }
    
    public string Name { get; set; } = string.Empty;  // Field name (ex. "Identité", "Détails")
    public string Description { get; set; }  = string.Empty; // group description
    public int DisplayOrder { get; set; } // order of display
    
    public Guid RegisterTypeId { get; set; } // reference to the register type
    
    
    public Guid? CreatorId { get; set; }
    public DateTimeOffset? CreatedAt { get; set; }
}

public record RegisterFieldGroupUpdatedEvent : INotification
{
    public Guid Id { get; set; }
    public bool IsDeleted { get; set; }
    
    public string Name { get; set; } = string.Empty;  // Field name (ex. "Identité", "Détails")
    public string Description { get; set; }  = string.Empty; // group description
    public int DisplayOrder { get; set; } // order of display
    
    public Guid RegisterTypeId { get; set; } // reference to the register type
    
    
    public Guid? CreatorId { get; set; }
    public DateTimeOffset? CreatedAt { get; set; }
    
    public Guid? DeleterId { get; set; }
    public DateTimeOffset? DeletedAt { get; set; }
    
    public Guid? UpdaterId { get; set; }
    public DateTimeOffset? UpdatedAt { get; set; }
}
public record RegisterFieldGroupDeletedEvent(Guid Id) : INotification;