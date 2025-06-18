using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Soditech.IntelPrev.Reports.Shared;
using Soditech.IntelPrev.Reports.Shared.Alerts;

namespace Soditech.IntelPrev.Web.Pages.Administration.Alerts;

public partial class AlertDetails : ComponentBase
{
    [Parameter]
    public string? Id { get; set; }
    private AlertResult? SelectedAlert { get; set; }
    private const string SelectedAlertCacheKey = "SelectedAlertCacheKey";
    private bool IsLoading = false;
    protected override async Task OnInitializedAsync()
    {
        await LoadAlertAsync();
    }
    private async Task LoadAlertFromApiAsync()
    {
        IsLoading = true;

        var alertResult = await ProxyService.GetAsync<AlertResult>(ReportRoutes.Alerts.GetById.Replace("{id:guid}", Id));

        if (alertResult.IsSuccess)
        {
            SelectedAlert = alertResult.Value;
        }

        IsLoading = false;
    }
    private void GoBack()
    {
        Navigation.NavigateTo("/alerts");
    }

    private async Task LoadAlertAsync()
    {
        IsLoading = true;
        var (exists, cachedValue) = CacheService.Get(SelectedAlertCacheKey);

        if (exists)
        {
            SelectedAlert = (AlertResult)cachedValue;
        }
        else
        {
            await LoadAlertFromApiAsync();
        }
        IsLoading = false;
    }
}