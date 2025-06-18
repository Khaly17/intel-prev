using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Soditech.IntelPrev.Reports.Shared;
using Soditech.IntelPrev.Reports.Shared.RegisterTypes;
using Soditech.IntelPrev.Reports.Shared.Reports;

namespace Soditech.IntelPrev.Web.Components.Administrations.Registers;

public partial class CreateReportComponent : ComponentBase
{
    private CreateReportCommand CreateReport { get; set; } = default!;
    private RegisterTypeResult _registerType = default!;
    private bool _isBusy;


    /// <inheritdoc />
    protected override async Task OnInitializedAsync()
    {
        _isBusy = true;
        var registerTypeResult = await ProxyService.GetAsync<RegisterTypeResult>(ReportRoutes.RegisterTypes
            .GetById.Replace("{id:guid}", "30a8111e-57b0-4b66-84b2-1f82d4d0b7e8"));

        if (registerTypeResult.IsSuccess)
        {
            _registerType = registerTypeResult.Value;
            CreateReport = new CreateReportCommand(_registerType);
        }
        _isBusy = false;
    }
    
    private async Task CreateReportAsync()
    {
        //_isBusy = true;
        CreateReport.UpdateCreateReportDataCommand();
        var reportResult = await ProxyService.PostAsync<ReportResult>(ReportRoutes.Reports.Create, CreateReport);

        if (reportResult.IsSuccess)
        {
            Console.WriteLine("Report created");
        }
        else
        {
            Console.WriteLine("Error while creating report:" + reportResult.Error.Message);
        }
        _isBusy = false;
    }
}