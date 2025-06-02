using Microsoft.AspNetCore.Components;
using Soditech.IntelPrev.Reports.Shared.ReportDatas;
using Syncfusion.Blazor.Charts.Chart.Internal;

namespace Soditech.IntelPrev.Web.Shared.widgets;

public partial class SfAccumulationChartComponent
{
    [Parameter] 
    public IEnumerable<ReportDataResult> PieChartData { get; set; } = default!;
}
