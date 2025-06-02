using MediatR;

namespace Soditech.IntelPrev.Reports.Shared.RegisterFields;

public record RegisterFieldCreatedEvent : INotification
{
    public Guid Id { get; set; }
    public bool IsDeleted { get; set; }
    
    public string Name { get; set; } = string.Empty;  // Field name (ex. Victime, Lieu)
    public string Description { get; set; } = string.Empty;  // Field description
    public string FieldType { get; set; } = string.Empty; // Field type (texte, date, booléen, etc.)
    public bool IsRequired { get; set; } // Required or not
    public int DisplayOrder { get; set; } // Display order
    
    public Guid RegisterTypeId { get; set; } // reference to the register type
    
    public Guid? RegisterFieldGroupId { get; set; } // reference to the register type
    
    
    public Guid? CreatorId { get; set; }
    public DateTimeOffset? CreatedAt { get; set; }
}

public record RegisterFieldUpdatedEvent : INotification
{
    public Guid Id { get; set; }
    public bool IsDeleted { get; set; }
    
    public string Name { get; set; } = string.Empty;  // Field name (ex. Victime, Lieu)
    public string Description { get; set; } = string.Empty;  // Field description
    public string FieldType { get; set; } = string.Empty; // Field type (texte, date, booléen, etc.)
    public bool IsRequired { get; set; } // Required or not
    public int DisplayOrder { get; set; } // Display order
    
    public Guid RegisterTypeId { get; set; } // reference to the register type
    
    public Guid? RegisterFieldGroupId { get; set; } // reference to the register type

    
    
    public Guid? CreatorId { get; set; }
    public DateTimeOffset? CreatedAt { get; set; }
    
    public Guid? DeleterId { get; set; }
    public DateTimeOffset? DeletedAt { get; set; }
    
    public Guid? UpdaterId { get; set; }
    public DateTimeOffset? UpdatedAt { get; set; }
}
public record RegisterFieldDeletedEvent(Guid Id) : INotification;