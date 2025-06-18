using System;

namespace Soditech.IntelPrev.Web.Models;

public class RoleModel
{
    public Guid Id { get; set; }
    public string? Name { get; set; }
    public bool IsSelected { get; set; }
}
