﻿using System.Collections.Generic;
using System.Linq;

namespace Soditech.IntelPrev.Notifications.Shared.Models;

public class NotificationRequest
{
    public string Title { get; set; } = "IntelPrev";
    public string Text { get; set; } = string.Empty;
    public string Action { get; set; } = string.Empty;

    private string[] _tags = [];


    public IReadOnlyList<string> Tags
    {
        get => _tags;
        set => _tags = value?.ToArray() ?? [];
    }


    public bool Silent { get; set; }
}


//notification tags enum
public static class NotificationTags
{
    /// <summary>
    /// use this tag to send notifications to all users in the system.
    /// </summary>
    public const string All = "all";
    /// <summary>
    /// use this tag to send notifications to all users in the tenant.
    /// Replace $tenantId with the actual tenant id.
    /// </summary>
    public const string TenantUsers = "tenant:$tenantId";
    /// <summary>
    /// use this tag to send notifications to all users in the building.
    /// replace $buildingId with the actual building id.
    /// </summary>
    public const string BuildingUsers = "building:$buildingId";
    /// <summary>
    /// use this tag to send notifications to the specified user.
    /// </summary>
    public const string Users = "user:$userId";
    /// <summary>
    /// use this tag to send notifications to all users in the host tenant.
    /// </summary>
    public const string HostUsers = "hostUsers";
    /// <summary>
    /// replace $roleId with the actual role id.
    /// </summary>
    public const string Roles = "role:$roleId";
}