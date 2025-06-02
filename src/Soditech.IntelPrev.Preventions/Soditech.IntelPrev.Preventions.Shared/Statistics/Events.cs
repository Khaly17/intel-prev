using MediatR;

namespace Soditech.IntelPrev.Preventions.Shared.Statistics;

public record StatisticCreatedEvent : INotification
{
    public Guid Id { get; set; }
    public bool IsDeleted { get; set; }
    
    public string Category { get; set; } = string.Empty;
    public float Value { get; set; }
    public string Notes { get; set; } = string.Empty;
    public DateTimeOffset Date { get; set; }
    
    public Guid CampaignId { get; set; }
    
    public Guid? CreatorId { get; set; }
    public DateTimeOffset? CreatedAt { get; set; }
}

public record StatisticUpdatedEvent : INotification
{
    public Guid Id { get; set; }
    public bool IsDeleted { get; set; }
    
    public string Category { get; set; } = string.Empty;
    public float Value { get; set; }
    public string Notes { get; set; } = string.Empty;
    public DateTimeOffset Date { get; set; }
    
    public Guid CampaignId { get; set; }
    
    public Guid? CreatorId { get; set; }
    public DateTimeOffset? CreatedAt { get; set; }
    
    public Guid? DeleterId { get; set; }
    public DateTimeOffset? DeletedAt { get; set; }
    
    public Guid? UpdaterId { get; set; }
    public DateTimeOffset? UpdatedAt { get; set; }
}

public record StatisticDeletedEvent(Guid Id) : INotification;