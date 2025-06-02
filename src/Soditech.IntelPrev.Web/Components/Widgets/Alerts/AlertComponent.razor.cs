using Microsoft.AspNetCore.Components;

namespace Soditech.IntelPrev.Web.Components.Widgets.Alerts;

public partial class AlertComponent
{
    [Parameter] public string Message { get; set; } = string.Empty;
    [Parameter] public string AlertType { get; set; } = "success";
    [Parameter] public bool IsVisible { get; set; }
}