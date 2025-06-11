using Microsoft.AspNetCore.Components;
using Soditech.IntelPrev.Preventions.Shared.Campaigns;
using Soditech.IntelPrev.Preventions.Shared;
using Soditech.IntelPrev.Mediatheques.Shared;
using Soditech.IntelPrev.Users.Shared.Users;

namespace Soditech.IntelPrev.Web.Pages.Administration.Campaigns;
public partial class EditCampaign
{
    [Parameter]
    public string CampaignId { get; set; } = string.Empty;
    public CampaignResult currentCampaign { get; set; } = new();
    public string title { get; set; } = "Modifier la campagne";
    private string? successMessage;
    private string? errorMessage;
    private bool IsLoading = false;
    private const string CampaignsCacheKey = "Campaigns";
    private string GetCampaignCacheKey() => $"Campaign_{CampaignId}";

    protected override async Task OnInitializedAsync()
    {
        await LoadCampaignsAsync();
    }

    private async Task LoadCampaignsAsync()
    {
        IsLoading = true;
        var cacheKey = GetCampaignCacheKey();
        var (exists, cachedValue) = CacheService.Get(cacheKey);

        if (exists)
        {
            currentCampaign = (CampaignResult)cachedValue;
        }
        else
        {
            await LoadCurrentCampaignFromApiAsync();
            CacheService.Set(cacheKey, currentCampaign);
        }
        IsLoading = false;
    }
    private async Task LoadCurrentCampaignFromApiAsync()
    {
        try
        {
            IsLoading = true;
            var result = await ProxyService.GetAsync<CampaignResult>(PreventionRoutes.Campaigns.GetById.Replace("{id:guid}", CampaignId));

            if (result.IsSuccess)
            {
                currentCampaign = result.Value;

            }
            else
            {
                errorMessage = "Erreur de récupération des informations de la campagne.";
            }
        }
        catch (Exception ex)
        {
            errorMessage = $"Erreur: {ex.Message}";
        }
        IsLoading = false;
    }
    private async Task UpdateCampaign()
    {
        if (currentCampaign.Id == Guid.Empty)
        {
            errorMessage = "L'ID de la campagne est invalide.";
            return;
        }

        var updateResult = await ProxyService.PostAsync<CampaignResult>(PreventionRoutes.Campaigns.Update.Replace("{id:guid}", currentCampaign.Id.ToString()), currentCampaign);
        if (updateResult.IsSuccess)
        {
            successMessage = "Informations mis à jour avec succès.";
            errorMessage = null;
            CacheService.Set(GetCampaignCacheKey(), currentCampaign);
            CacheService.Set(CampaignsCacheKey, null);
            Navigation.NavigateTo("/campaigns");
        }
        else
        {
            errorMessage = "Erreur lors de la mise à jour des informations de la campagne.";
        }
    }

}
