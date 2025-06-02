namespace Soditech.IntelPrev.Users.Shared.Enums;

public enum RoleScope
{
    /// <summary>
    /// Application scope is for roles that aren't linked to an application
    /// </summary>
    Application,
    /// <summary>
    /// Tenant scope is for roles that are linked to an structure (organization, company, etc.)
    /// </summary>
    Tenant,
}
