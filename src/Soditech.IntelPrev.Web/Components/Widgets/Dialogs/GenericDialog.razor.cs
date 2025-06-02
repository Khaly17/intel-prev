using Microsoft.AspNetCore.Components;
using Syncfusion.Blazor.Popups;

namespace Soditech.IntelPrev.Web.Components.Widgets.Dialogs;

public partial class GenericDialog<TItem> : ComponentBase
{
    [Parameter] public bool IsVisible { get; set; }
    [Parameter] public EventCallback<bool> IsVisibleChanged { get; set; }
    [Parameter] public string HeaderTitle { get; set; } = "Dialog";
    [Parameter] public RenderFragment? ChildContent { get; set; }
    [Parameter] public EventCallback SaveCallback { get; set; }
    [Parameter] public EventCallback CancelCallback { get; set; }

    private void Save()
    {
        SaveCallback.InvokeAsync();
    }

    private void Cancel()
    {
        CancelCallback.InvokeAsync();
        IsVisibleChanged.InvokeAsync(false);
    }


}