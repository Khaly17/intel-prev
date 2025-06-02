using Microsoft.AspNetCore.Components;
using Soditech.IntelPrev.Reports.Shared.Reports;
using Soditech.IntelPrev.Reports.Shared;
using Soditech.IntelPrev.Web.Services.Proxy;

namespace Soditech.IntelPrev.Web.Pages.Administration.Reports;

public partial class ReportDetails: ComponentBase
{
    [Parameter]
    public string? Id { get; set; }
    private ReportResult? SelectedReport { get; set; }
    private const string SelectedReportCacheKey = "SelectedReportCacheKey";
    private bool IsLoading = false;
    protected override async Task OnInitializedAsync()
    {
        await LoadReportAsync();
    }

    private async Task LoadReportFromApiAsync()
    {
        IsLoading = true;

        var reportResult = await ProxyService.GetAsync<ReportResult>(ReportRoutes.Reports.GetById.Replace("{id:guid}", Id));

        if (reportResult.IsSuccess)
        {
            SelectedReport = reportResult.Value;
        }

        IsLoading = false;
    }

    private void GoBack()
    {
        Navigation.NavigateTo("/report-details");
    }

    private string GetStatusClass(string status) => status switch
    {
        "Open" => "status-open",
        "Closed" => "status-closed",
        _ => "status-default"
    };
    private async Task LoadReportAsync()
    {
        IsLoading = true;
        var (exists, cachedValue) = CacheService.Get(SelectedReportCacheKey);

        if (exists)
        {
            SelectedReport = (ReportResult)cachedValue;
        }
        else
        {
            await LoadReportFromApiAsync();
        }
        IsLoading = false;
    }
}
