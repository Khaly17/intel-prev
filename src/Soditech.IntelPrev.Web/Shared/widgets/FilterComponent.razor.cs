using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components;

namespace Soditech.IntelPrev.Web.Shared.widgets;

public partial class FilterComponent : ComponentBase
{
    [Parameter] public EventCallback SetCurrentWeek { get; set; }
    [Parameter] public EventCallback SetCurrentMonth { get; set; }
    [Parameter] public DateTime StartDate { get; set; }
    [Parameter] public DateTime EndDate { get; set; }
    [Parameter] public EventCallback<DateTime> StartDateChanged { get; set; }
    [Parameter] public EventCallback<DateTime> EndDateChanged { get; set; }
    [Parameter] public EventCallback LoadCharts { get; set; }
    [Parameter] public EventCallback Print { get; set; }
    [Parameter] public EventCallback Download { get; set; }
    [Parameter] public EventCallback Notify { get; set; }

    private Task OnStartDateChanged(DateTime newStartDate)
    {
        StartDate = newStartDate;
        return StartDateChanged.InvokeAsync(StartDate);
    }

    private Task OnEndDateChanged(DateTime newEndDate)
    {
        EndDate = newEndDate;
        return EndDateChanged.InvokeAsync(EndDate);
    }

    private async Task HandleKeyDown(KeyboardEventArgs args)
    {
        if (args.Key == "Enter")
        {
            await LoadCharts.InvokeAsync(null);
        }
    }
}
