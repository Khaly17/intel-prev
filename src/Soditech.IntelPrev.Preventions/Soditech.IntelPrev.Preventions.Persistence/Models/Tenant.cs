using Sensor6ty.Domain;

namespace Soditech.IntelPrev.Preventions.Persistence.Models;

public class Tenant : EntityBase
{
    /// <summary>
    /// this is the normalized name. it must be in upper case. 
    /// </summary>
    public string Name { get; set; } = string.Empty;
    public string DisplayName { get; set; } = string.Empty;
    public bool IsActive { get; set; }
}