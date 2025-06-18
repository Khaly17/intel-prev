using System;
using System.Collections.Generic;
using Sensor6ty.Domain;

namespace Soditech.IntelPrev.Users.Persistence.Models;

public class Building : EntityBase
{
    public string Name { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;

    public virtual List<User> Users { get; set; } = default!;

    public virtual List<Floor> Floors { get; set; } = default!;

    public Guid TenantId { get; set; }
    public virtual Tenant Tenant { get; set; } = default!;
}