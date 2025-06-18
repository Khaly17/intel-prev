using System;
using System.Collections.Generic;
using Sensor6ty.Domain;

namespace Soditech.IntelPrev.Users.Persistence.Models;

public class Floor : EntityBase
{
    public int Number { get; set; }

    public Guid BuildingId { get; set; }
    public virtual Building Building { get; set; } = default!;

    public virtual IList<User> Users { get; set; } = default!;

    public Guid TenantId { get; set; }
    public virtual Tenant Tenant { get; set; } = default!;
}