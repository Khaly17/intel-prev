using MediatR;

namespace Soditech.IntelPrev.Users.Shared.Users.Events;

public record UserCreatedEvent : INotification
{
    public Guid Id { get; set; } 
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string UserName { get; set; } = string.Empty;   
    public string PhoneNumber { get; set; } = string.Empty;   
    public string Email { get; set; } = string.Empty;   
    
    public Guid? SiteId { get; set; }
    
    public Guid? TenantId { get; set; }
}

public record UserUpdatedEvent : INotification
{
    public Guid Id { get; set; } 
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string UserName { get; set; } = string.Empty;   
    public string PhoneNumber { get; set; } = string.Empty;   
    public string Email { get; set; } = string.Empty;   
    
    public Guid? SiteId { get; set; }
    
    public Guid? TenantId { get; set; }
}

public record UserDeletedEvent : INotification
{
    public Guid Id { get; set; } 
}