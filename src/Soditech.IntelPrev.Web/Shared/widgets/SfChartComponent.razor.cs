using System.Collections.Generic;
using Microsoft.AspNetCore.Components;
using Syncfusion.Blazor.Charts;

namespace Soditech.IntelPrev.Web.Shared.widgets;

public partial class SfChartComponent<TItem> : ComponentBase
{
    [Parameter]
    public string XName { get; set; } = string.Empty;

    [Parameter]
    public string YName { get; set; } = string.Empty;

    [Parameter]
    public IEnumerable<object> DataSource { get; set; } = new List<object>();

    private static string[] Palettes => ["#0b2850", "#1755aa", "#1729aa"];
    private int _i;

    public void PointRenderEvent(PointRenderEventArgs args)
    {
        if (_i >= Palettes.Length)
            _i = 0;
        args.Fill = Palettes[_i++];
    }
}