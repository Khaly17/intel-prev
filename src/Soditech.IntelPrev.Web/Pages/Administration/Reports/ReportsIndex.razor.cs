using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Logging;
using Syncfusion.Blazor.Grids;
using Soditech.IntelPrev.Reports.Shared.Reports;
using Soditech.IntelPrev.Reports.Shared;
using Soditech.IntelPrev.Web.Services.Extensions;
using Soditech.IntelPrev.Web.Models;

namespace Soditech.IntelPrev.Web.Pages.Administration.Reports;

public partial class ReportsIndex : ComponentBase
{
    [Parameter]
    [SupplyParameterFromQuery(Name = "StartDate")]
    public string? StartDateParam { get; set; }

    [Parameter]
    [SupplyParameterFromQuery(Name = "EndDate")]
    public string? EndDateParam { get; set; }
    private DateFilter DateFilter { get; set; } = default!;
    public IList<ReportResult> ReportList { get; set; } = new List<ReportResult>();
    private int PageCount { get; set; } = 10;
    private int PageSize { get; set; } = 10;
    private bool IsLoading { get; set; }

    private const string SelectedReportCacheKey = "SelectedReportCacheKey";


    private static List<GridColumn> Columns =>
    [
        new GridColumn { Field = nameof(ReportResult.Title), HeaderText = "Titre" },
        new GridColumn { Field = nameof(ReportResult.Status), HeaderText = "Statut" },
        new GridColumn
        {
            Field = nameof(ReportResult.CreatedAt),
            HeaderText = "Date",
            Format = "dd/MM/yyyy"
        },

        new GridColumn
        {
            Field = nameof(ReportResult.CreatedAt),
            HeaderText = "Heure",
            Format = "HH:mm"
        },


        new GridColumn { Field = nameof(ReportResult.CreatorFullName), HeaderText = "Signalé par" }

    ];

    private static List<string> ToolbarItems => ["Search", "ExcelExport", "PdfExport", "Print"];

    private bool _isDeleteModalVisible;
    private string _alertMessage = string.Empty;
    private string _alertType = "success";
    private bool _isAlertVisible;
    private ReportResult SelectedReport { get; set; } = default!;

    [Inject] private ILogger<ReportsIndex> Logger { get; set; } = default!;

    protected override async Task OnInitializedAsync()
    {
        DateFilter = new DateFilter
        {
            StartDate = DateTime.TryParse(StartDateParam, out var startDate) ? startDate : DateTime.Today,
            EndDate = DateTime.TryParse(EndDateParam, out var endDate) ? endDate : DateTime.Today
        };
        await GetReports();
    }

    private async Task GetReports()
    {
        IsLoading = true;

        var reportsResult = await ProxyService.GetAsync<IList<ReportResult>>(
            ReportRoutes
            .Reports
            .GetAll
            .AddQueryParameters(DateFilter));

        if (reportsResult.IsSuccess)
        {
            ReportList = reportsResult.Value;
        }

        IsLoading = false;
    }

    private void HideDeleteModal() => _isDeleteModalVisible = false;

    private async Task DeleteReportAsync()
    {
        try
        {
            var result = await ProxyService.DeleteAsync(ReportRoutes.Reports.Delete.Replace("{id:guid}", SelectedReport.Id.ToString()));

            if (result.IsSuccess)
            {
                ReportList = ReportList.Where(n => n.Id != SelectedReport.Id).ToList();
                ShowAlert("Report deleted successfully.");
            }
            else
            {
                ShowAlert("Failed to delete report.", "danger");
            }
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, "Error deleting report: {Message}", ex.Message);
            ShowAlert("An error occurred while deleting the report. Please try again.", "danger");
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

    private void GoToDetails(ReportResult report)
    {
        CacheService.Set(SelectedReportCacheKey, report);
        Navigation.NavigateTo($"reports/detail/{report.Id}");
    }

}