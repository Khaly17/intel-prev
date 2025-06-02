using Microsoft.AspNetCore.Components;

namespace Soditech.IntelPrev.Web.Shared.widgets;

public partial class CircularGaugeComponent: ComponentBase
{
    [Parameter] 
    public double Value { get; set; }
    [Parameter] 
    public string OkValue { get; set; }
    [Parameter] 
    public string NcValue { get; set; }
    public string FormattedValue => $"{Value}%";
}
