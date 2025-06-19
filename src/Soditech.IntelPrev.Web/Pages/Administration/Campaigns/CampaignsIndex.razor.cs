using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Logging;
using Soditech.IntelPrev.Prevensions.Shared;
using Soditech.IntelPrev.Prevensions.Shared.Campaigns;
using Syncfusion.Blazor.Grids;

namespace Soditech.IntelPrev.Web.Pages.Administration.Campaigns;

public partial class CampaignsIndex
{
    [Inject] private ILogger<CampaignsIndex> Logger { get; set; } = default!;
    private IList<CampaignResult> CampaignResults { get; set; } = default!;
    private int PageCount { get; set; } = 10;
    private int PageSize { get; set; } = 10;
    private bool IsLoading { get; set; }

    private static List<GridColumn> Columns =>
    [       
        new() { Field =  nameof(CampaignResult.Name), HeaderText = "Nom" },
        new() { Field = nameof(CampaignResult.Description), HeaderText = "Description" },
        new() { Field = nameof(CampaignResult.StartDate), HeaderText = "Date de début",Format = "dd/MM/yyyy" },
        new() { Field = nameof(CampaignResult.EndDate), HeaderText = "Date de fin",Format = "dd/MM/yyyy" }
    ];

    private static List<string> ToolbarItems => ["Search", "ExcelExport", "PdfExport", "Print"];

    private bool _isDeleteModalVisible;
    private string _alertMessage = string.Empty;
    private string _alertType = "success";
    private bool _isAlertVisible;
    private readonly bool _isDisableCampaignModalVisible;

    private const string CampaignsCacheKey = "Campaigns";
    private CampaignResult Selectedcampaign { get; set; } = default!;

    protected override async Task OnInitializedAsync()
    {
        await LoadCampaignsAsync();
    }


    private void AddCampaign()
    {
        Navigation.NavigateTo("/campaigns/add");
    }
    private void GoToEdit(CampaignResult campaign)
    {
        Navigation.NavigateTo($"campaigns/edit/{campaign.Id}");
    }

    private async Task LoadCampaignsAsync()
    {
        IsLoading = true;
        var (exists, cachedValue) = CacheService.Get(CampaignsCacheKey);

        if (exists)
        {
            CampaignResults = (IList<CampaignResult>)cachedValue;
        }
        else
        {
            await LoadCampaignsFromApiAsync();
            CacheService.Set(CampaignsCacheKey, CampaignResults);
        }
        IsLoading = false;
    }
    private async Task LoadCampaignsFromApiAsync()
    {

        IsLoading = true;
        var campaignsResult = await ProxyService.GetAsync<IList<CampaignResult>>(PreventionRoutes.Campaigns.GetAll);

        if (campaignsResult.IsSuccess)
        {
            CampaignResults = campaignsResult.Value;
        }
        IsLoading = false;
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

    private void DeleteCampaign(CampaignResult campaign)
    {
        Selectedcampaign = campaign;
        _isDeleteModalVisible = true;
    }
    private void HideDeleteModal() => _isDeleteModalVisible = false;

    private async Task DeleteCampaignAsync()
    {
        try
        {
            var result = await ProxyService.DeleteAsync(PreventionRoutes.Campaigns.Delete.Replace("{id:guid}", Selectedcampaign.Id.ToString()));

            if (result.IsSuccess)
            {
                CampaignResults = CampaignResults.Where(n => n.Id != Selectedcampaign.Id).ToList();
                ShowAlert("Campagne supprimé avec succès.");
            }
            else
            {
                ShowAlert("Échec de la suppression du cmpagne.", "danger");
            }

        }
        catch (Exception ex)
        {
            Logger.LogError(ex, "Erreur lors de la suppression du campagne : {Message}", ex.Message);
            ShowAlert("Une erreur s'est produite lors de la suppression du campagne. Veuillez réessayer.", "danger");
        }
        finally
        {
            HideDeleteModal();
        }
    }

}
