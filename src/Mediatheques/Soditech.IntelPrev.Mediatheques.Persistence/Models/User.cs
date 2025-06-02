using Sensor6ty.Domain;

namespace Soditech.IntelPrev.Mediatheques.Persistence.Models;

public class User : EntityBase
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string FullName => $"{FirstName} {LastName}";
    public string Email { get; set; } = string.Empty;
    public string Username { get; set; } = string.Empty;
    public virtual List<Document> Documents { get; set; } = [];
    
    public Guid? TenantId { get; set; }
    public virtual Tenant? Tenant { get; set; }
}