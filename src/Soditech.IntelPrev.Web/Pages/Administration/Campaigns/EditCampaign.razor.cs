using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Soditech.IntelPrev.Prevensions.Shared;
using Soditech.IntelPrev.Prevensions.Shared.Campaigns;

namespace Soditech.IntelPrev.Web.Pages.Administration.Campaigns;
public partial class EditCampaign
{
    [Parameter]
    public string CampaignId { get; set; } = string.Empty;
    public CampaignResult CurrentCampaign { get; set; } = new();
    public string Title { get; set; } = "Modifier la campagne";
    private string? _successMessage;
    private string? _errorMessage;
    private bool _isLoading = false;
    private const string CampaignsCacheKey = "Campaigns";
    private string GetCampaignCacheKey() => $"Campaign_{CampaignId}";

    protected override async Task OnInitializedAsync()
    {
        await LoadCampaignsAsync();
    }

    private async Task LoadCampaignsAsync()
    {
        _isLoading = true;
        var cacheKey = GetCampaignCacheKey();
        var (exists, cachedValue) = CacheService.Get(cacheKey);

        if (exists)
        {
            CurrentCampaign = (CampaignResult)cachedValue;
        }
        else
        {
            await LoadCurrentCampaignFromApiAsync();
            CacheService.Set(cacheKey, CurrentCampaign);
        }
        _isLoading = false;
    }
    private async Task LoadCurrentCampaignFromApiAsync()
    {
        try
        {
            _isLoading = true;
            var result = await ProxyService.GetAsync<CampaignResult>(PreventionRoutes.Campaigns.GetById.Replace("{id:guid}", CampaignId));

            if (result.IsSuccess)
            {
                CurrentCampaign = result.Value;

            }
            else
            {
                _errorMessage = "Erreur de récupération des informations de la campagne.";
            }
        }
        catch (Exception ex)
        {
            _errorMessage = $"Erreur: {ex.Message}";
        }
        _isLoading = false;
    }
    private async Task UpdateCampaignAsync()
    {
        if (CurrentCampaign.Id == Guid.Empty)
        {
            _errorMessage = "L'ID de la campagne est invalide.";
            return;
        }

        var updateResult = await ProxyService.PostAsync<CampaignResult>(PreventionRoutes.Campaigns.Update.Replace("{id:guid}", CurrentCampaign.Id.ToString()), CurrentCampaign);
        if (updateResult.IsSuccess)
        {
            _successMessage = "Informations mis à jour avec succès.";
            _errorMessage = null;
            CacheService.Set(GetCampaignCacheKey(), CurrentCampaign);
            CacheService.Set(CampaignsCacheKey, null);
            Navigation.NavigateTo("/campaigns");
        }
        else
        {
            _errorMessage = "Erreur lors de la mise à jour des informations de la campagne.";
        }
    }

}
