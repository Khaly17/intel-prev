using System.Collections.Generic;
using Microsoft.AspNetCore.Components;
using Soditech.IntelPrev.Reports.Shared.ReportDatas;

namespace Soditech.IntelPrev.Web.Shared.widgets;

public partial class SfAccumulationChartComponent
{
    [Parameter] 
    public IEnumerable<ReportDataResult> PieChartData { get; set; } = default!;
}
