using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Logging;
using Syncfusion.Blazor.Grids;
using Soditech.IntelPrev.Reports.Shared.Alerts;
using Soditech.IntelPrev.Web.Services.Extensions;
using Soditech.IntelPrev.Web.Models;
using Soditech.IntelPrev.Reports.Shared;

namespace Soditech.IntelPrev.Web.Pages.Administration.Alerts;

public partial class AlertsIndex : ComponentBase
{
    [Parameter]
    [SupplyParameterFromQuery(Name = "StartDate")]
    public string? StartDateParam { get; set; }

    [Parameter]
    [SupplyParameterFromQuery(Name = "EndDate")]
    public string? EndDateParam { get; set; }
    private DateFilter DateFilter { get; set; } = default!;
    public IList<AlertResult> AlertList { get; set; } = new List<AlertResult>();
    private int PageCount { get; set; } = 10;
    private int PageSize { get; set; } = 10;
    private bool IsLoading { get; set; }

    private const string SelectedAlertCacheKey = "SelectedAlertCacheKey";
    private static List<GridColumn> Columns =>
    [
        new GridColumn { Field = nameof(AlertResult.Title), HeaderText = "Titre" },
        new GridColumn { Field = nameof(AlertResult.Description), HeaderText = "Description" },
        new GridColumn { Field = nameof(AlertResult.Type), HeaderText = "Type" }
    ];

    private static List<string> ToolbarItems => ["Search", "ExcelExport", "PdfExport", "Print"];

    private bool _isDeleteModalVisible;
    private string _alertMessage = string.Empty;
    private string _alertType = "success";
    private bool _isAlertVisible;
    private AlertResult SelectedAlert { get; set; } = default!;

    [Inject] private ILogger<AlertsIndex> Logger { get; set; } = default!;

    protected override async Task OnInitializedAsync()
    {
        DateFilter = new DateFilter
        {
            StartDate = DateTime.TryParse(StartDateParam, out var startDate) ? startDate : DateTime.Today,
            EndDate = DateTime.TryParse(EndDateParam, out var endDate) ? endDate : DateTime.Today
        };
        await GetAlerts();
    }

    private async Task GetAlerts()
    {
        IsLoading = true;

        var alertsResult = await ProxyService.GetAsync<IList<AlertResult>>(
            ReportRoutes
            .Alerts
            .GetAlertsByPeriod
            .AddQueryParameters(DateFilter));

        if (alertsResult.IsSuccess)
        {
            AlertList = alertsResult.Value;
        }

        IsLoading = false;
    }
    private void GoToDetails(AlertResult alert)
    {
        CacheService.Set(SelectedAlertCacheKey, alert);
        Navigation.NavigateTo($"/alerts/detail/{alert.Id}");
    }
    private void DeleteAlertTrigger(AlertResult alert)
    {
        SelectedAlert = alert;
        _isDeleteModalVisible = true;
    }

    private void HideDeleteModal() => _isDeleteModalVisible = false;

    private void AddAlert()
    {
        Navigation.NavigateTo("/alerts/add");
    }

    private async Task DeleteAlertAsync()
    {
        try
        {
            var result = await ProxyService.DeleteAsync(ReportRoutes.Alerts.Delete.Replace("{id:guid}", SelectedAlert.Id.ToString()));

            if (result.IsSuccess)
            {
                AlertList = AlertList.Where(n => n.Id != SelectedAlert.Id).ToList();
                ShowAlert("Alert deleted successfully.");
            }
            else
            {
                ShowAlert("Failed to delete alert.", "danger");
            }
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, "Error deleting alert: {Message}", ex.Message);
            ShowAlert("An error occurred while deleting the alert. Please try again.", "danger");
        }
        finally
        {
            HideDeleteModal();
        }
    }

    private void ShowAlert(string message, string type = "success")
    {
        _alertMessage = message;
        _alertType = type;
        _isAlertVisible = true;
        Task.Delay(3000).ContinueWith(_ =>
        {
            _isAlertVisible = false;
            StateHasChanged();
        });
    }

    private void GoToEdit(AlertResult alert)
    {
        Navigation.NavigateTo($"alerts/edit/{alert.Id}");
    }
}