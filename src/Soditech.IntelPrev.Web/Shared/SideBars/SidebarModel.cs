using System.Collections.Generic;

namespace Soditech.IntelPrev.Web.Shared.SideBars;

public class SidebarSection
{
    public SidebarSection()
    {
    }

    public SidebarSection(string? name, string? iconClass, List<SidebarItem> items)
    {
        Name = name;
        IconClass = iconClass;
        Items = items;
    }

    public string? Name { get; set; }
    public string? IconClass { get; set; }
    public List<SidebarItem> Items { get; set; } = default!;
    public bool IsExpanded { get; set; } = true;

}

public class SidebarItem
{
    public string Key { get; set; } = string.Empty;
    public string? Name { get; set; }
    public string? IconClass { get; set; }
    public string? Url { get; set; }
    public bool IsTenant { get; set; }
    public bool IsHost { get; set; }
}
